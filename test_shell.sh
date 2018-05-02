#!/bin/sh

# Fetch argments defined in Jenkins.
echo "version = $Version"
echo "IsForDev = $IsForDev"

echo "$gitlabBranch"
echo "$gitlabSourceBranch"
echo $gitlabActionType
echo $gitlabUserName
echo $gitlabUserEmail
echo $GIT_COMMIT