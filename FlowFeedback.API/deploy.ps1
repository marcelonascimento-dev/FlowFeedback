<#
.SYNOPSIS
    Script robusto de deploy para Azure App Service utilizando ZipDeploy.
.DESCRIPTION
    1. Gera os artefatos via dotnet publish.
    2. Compacta os arquivos em um ZIP temporário para evitar erros de permissão.
    3. Realiza o deploy via Azure CLI.
#>

param (
    [Parameter(Mandatory=$true)]
    [string]$resourceGroup,

    [Parameter(Mandatory=$true)]
    [string]$appName,

    [Parameter(Mandatory=$false)]
    [string]$configuration = "Release"
)

# Configura o script para parar se houver erro
$ErrorActionPreference = "Stop"

# Define caminhos absolutos baseados na localização do script
$rootPath = Get-Location
$outputDir = Join-Path -Path $rootPath -ChildPath "publish_output"
$zipFile = Join-Path -Path $rootPath -ChildPath "deploy_package.zip"

Write-Host "`n🚀 Iniciando processo de deploy..." -ForegroundColor Cyan

# 1. Limpeza de deploys anteriores
if (Test-Path $outputDir) {
    Write-Host "🧹 Removendo pasta de saída antiga..." -ForegroundColor Gray
    Remove-Item -Recurse -Force $outputDir
}
if (Test-Path $zipFile) {
    Remove-Item -Force $zipFile
}

# 2. Build e Publish
Write-Host "📦 Gerando artefatos (.NET 10) em: $outputDir" -ForegroundColor Cyan
dotnet publish -c $configuration -o $outputDir --self-contained false -r linux-x64 -p:PublishReadyToRun=true

# 3. Verificação de integridade
if (!(Test-Path "$outputDir\web.config") -and !(Test-Path "$outputDir\appsettings.json")) {
    Write-Error "❌ Falha crítica: Os arquivos de publicação não foram encontrados em $outputDir"
}

# 4. Compactação (Resolve erro de 'permissions' do Azure CLI)
Write-Host "🗜️ Criando pacote compactado..." -ForegroundColor Cyan
# O uso de -Path "$outputDir\*" garante que os arquivos fiquem na raiz do ZIP
Compress-Archive -Path "$outputDir\*" -DestinationPath $zipFile -Force

# 5. Deploy para o Azure
Write-Host "☁️ Enviando para o Azure App Service: [$appName]..." -ForegroundColor Cyan

# Valida se o usuário está logado
try {
    az account show --query "name" -o tsv | Out-Null
} catch {
    Write-Error "❌ Você não está logado na Azure CLI. Execute 'az login' primeiro."
}

# Executa o deploy apontando para o arquivo ZIP
az webapp deploy --resource-group $resourceGroup `
                 --name $appName `
                 --src-path $zipFile `
                 --type zip

# 6. Limpeza pós-deploy
Write-Host "🗑️ Limpando arquivos temporários..." -ForegroundColor Gray
if (Test-Path $zipFile) { Remove-Item $zipFile -Force }

Write-Host "`n✅ Deploy concluído com sucesso!" -ForegroundColor Green