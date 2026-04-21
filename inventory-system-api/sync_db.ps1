# --- Configuration ---
$sqlPackagePath = "C:\Program Files\Microsoft Visual Studio\18\Community\Common7\IDE\Extensions\Microsoft\SQLDB\DAC\SqlPackage.exe"
$tempDacpac = "$env:TEMP\temp_schema.dacpac"

$sourceConn = "Data Source=.;Initial Catalog=INVENTORY_SYSTEM;Integrated Security=True;TrustServerCertificate=True"
$targetConn = "Data Source=.;Initial Catalog=INVENTORY_SYSTEM_TEST;Integrated Security=True;TrustServerCertificate=True"

Write-Host "--- Starting Database Sync ---" -ForegroundColor Cyan

try {
    # STEP 1: Extract (Original DB -> File)
    Write-Host "1. Extracting schema from OriginalDB..." -ForegroundColor Yellow
    & $sqlPackagePath /Action:Extract /SourceConnectionString:$sourceConn /TargetFile:$tempDacpac /p:IgnoreUserLoginMappings=True

    # STEP 2: Publish (File -> Test DB)
    Write-Host "2. Applying schema to TestDB..." -ForegroundColor Yellow
    & $sqlPackagePath /Action:Publish /SourceFile:$tempDacpac /TargetConnectionString:$targetConn `
        /p:DropObjectsNotInSource=True `
        /p:BlockOnPossibleDataLoss=False

    Write-Host "✅ Done! TestDB now matches OriginalDB." -ForegroundColor Green
}
catch {
    Write-Host "❌ Error occurred during sync." -ForegroundColor Red
    $PSItem.Exception.Message
}
finally {
    if (Test-Path $tempDacpac) { Remove-Item $tempDacpac }
}