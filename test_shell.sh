#!/bin/sh

# Fetch argments defined in Jenkins.
echo "version = $Version"
echo "IsForDev = $IsForDev"

export gitlabBranch

echo "$gitlabBranch"
echo "$gitlabSourceBranch"
echo $gitlabActionType
echo $gitlabUserName
echo $gitlabUserEmail
echo $gitlabSourceRepoHomepage
echo $gitlabSourceRepoName
echo $gitlabSourceNamespace
echo $gitlabSourceRepoURL
echo $gitlabSourceRepoSshUrl
echo $gitlabSourceRepoHttpUrl
echo $gitlabMergeRequestTitle
echo $gitlabMergeRequestDescription
echo $gitlabMergeRequestId
echo $gitlabMergeRequestIid
echo $gitlabMergeRequestState
echo $gitlabMergedByUser
echo $gitlabMergeRequestAssignee
echo $gitlabMergeRequestLastCommit
echo $gitlabMergeRequestTargetProjectId
echo $gitlabTargetBranch
echo $gitlabTargetRepoName
echo $gitlabTargetNamespace
echo $gitlabTargetRepoSshUrl
echo $gitlabTargetRepoHttpUrl
echo $gitlabBefore
echo $gitlabAfter
echo $gitlabTriggerPhrase