$length = 32
$chars = 'abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()-_=+'
$bytes = New-Object byte[] $length
[System.Security.Cryptography.RandomNumberGenerator]::Create().GetBytes($bytes)

$key = -join ($bytes | ForEach-Object { $chars[ $_ % $chars.Length ] })

$key

Write-Host "`nPressione ENTER para sair..."
Read-Host
