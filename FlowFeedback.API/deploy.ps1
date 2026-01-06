param (
    [string]$resourceGroup = "flowfeedback_group",
    [string]$appName = "flowfeedback",
    [string]$configuration = "Release"
)

$ErrorActionPreference = "Stop"

$rootPath = Get-Location
$outputDir = Join-Path -Path $rootPath -ChildPath "publish_output"
$zipFile = Join-Path -Path $rootPath -ChildPath "deploy_package.zip"

Write-Host "`nIniciando deploy para $appName..." -ForegroundColor Cyan

if (Test-Path $outputDir) {
    Remove-Item -Recurse -Force $outputDir
}
if (Test-Path $zipFile) {
    Remove-Item -Force $zipFile
}

Write-Host "Gerando build..." -ForegroundColor Cyan
dotnet publish -c $configuration -o $outputDir --self-contained false -r linux-x64 -p:PublishReadyToRun=true

if (!(Test-Path "$outputDir\web.config") -and !(Test-Path "$outputDir\appsettings.json")) {
    Write-Error "Arquivos de publicacao nao encontrados."
}

Write-Host "Compactando..." -ForegroundColor Cyan
Compress-Archive -Path "$outputDir\*" -DestinationPath $zipFile -Force

try {
    az account show --query "name" -o tsv | Out-Null
} catch {
    Write-Error "Execute 'az login' primeiro."
}

# Configura o Azure para rodar direto do ZIP (evita bloqueio de arquivos)
Write-Host "Configurando Run From Package..." -ForegroundColor Cyan
az webapp config appsettings set --resource-group $resourceGroup --name $appName --settings WEBSITE_RUN_FROM_PACKAGE="1" | Out-Null

try {
    Write-Host "Enviando pacote..." -ForegroundColor Cyan
    # Removemos o stop/start, pois o Run From Package gerencia o swap atomicamente
    az webapp deploy --resource-group $resourceGroup `
                     --name $appName `
                     --src-path $zipFile `
                     --type zip
}
catch {
    Write-Error "Erro no deploy: $_"
}

if (Test-Path $zipFile) { Remove-Item $zipFile -Force }

Write-Host "`nDeploy concluido!" -ForegroundColor Green