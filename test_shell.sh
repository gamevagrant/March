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

echo "Last commit=$LAST_COMMIT"

export COMMIT_MESSAGE=\"$(git log --format=oneline -n 1 $CIRCLE_SHA1)\"
echo $COMMIT_MESSAGE

export LAST_COMMIT=$GIT_COMMIT
echo "Last commit=$LAST_COMMIT"

echo $JENKINS_HOME
echo $WORKSPACE
echo $CHANGES_SINCE_LAST_BUILD