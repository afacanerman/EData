param($websiteName, $packOutput)

$websites = Get-AzureWebsite | where-object -FilterScript{$_.Name -eq $websiteName}
foreach ($website In $websites)
{
	# Stop website
	#Stop-AzureWebsite $website.Name
	
	# Send service stopped notification
	#$postSlackMessage = @{ 
     #   token="xoxp-12405527623-13511049575-20101286342-14fc8f3467";
      #  channel="#servicestatus";
       # text= "@here:  $($website.Name) $($action)ped :zzz:";
        #icon_emoji= ":wrench:";
       # username="Devel∞per";
    #}


	# get the scm url to use with MSDeploy.  By default this will be the second in the array
	#$msdeployurl = $website.EnabledHostNames[1]
	
	#$publishProperties = @{'WebPublishMethod'='MSDeploy';
    #                    'MSDeployServiceUrl'=$msdeployurl;
    #                    'DeployIisAppPath'=$website.Name;
    #                    'Username'=$website.PublishingUsername;
    #                    'Password'=$website.PublishingPassword}

	# Deploy packages
	#$publishScript = "${env:ProgramFiles(x86)}\Microsoft Visual Studio 14.0\Common7\IDE\Extensions\Microsoft\Web Tools\Publish\Scripts\default-publish.ps1"
	#. $publishScript -publishProperties $publishProperties  -packOutput $packOutput

	# Start website
	#Start-AzureWebsite $website.Name
	
	# Send service stopped notification
	#$postSlackMessage = @{ 
     #   token="xoxp-12405527623-13511049575-20101286342-14fc8f3467";
      #  channel="#servicestatus";
       # text= "@here:  $($website.Name) $($action)ed :metal:";
        #icon_emoji= ":wrench:";
         #username="Devel∞per";
    #}
}









