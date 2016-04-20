param 
(
    [string]$filePath= "",
    [string]$versionKey="version",
    [string]$versioningPattern="\d{1,2}\.\d{1,3}\.(\*|\d{1,5})" #default pattern
)

$json = (Get-Content $filePath -Raw) | ConvertFrom-Json

foreach ($property in $json.PSObject.Properties)
{
    
    if($property.Name -eq $versionKey)
    {
        $property.Value -match $versioningPattern | % {$_ -match $versioningPattern > $null; $match=$matches[1]}
        if($match -eq $null)
        {
            throw "$versionKey does not match with versioning pattern"
        }

        if($match -eq "*")
        {
            $property.Value = $property.Value-replace "\*", "1"
        }
        else
        {
            $intNum = $match -as [int]
            $intNum++
            $property.Value = $property.Value.Substring(0, $property.Value.LastIndexOf('.')+1) + $intNum
        }
       

        $json | ConvertTo-Json -depth 999 | Out-File $filePath
    } 
        
}

