#!/bin/sh

# Fetch argments defined in Jenkins.
echo $gitlabActionType
echo $gitlabUserName
echo $GIT_COMMIT

export COMMIT_MESSAGE=\"$(git log --format=oneline -n 1 $CIRCLE_SHA1)\"
echo $COMMIT_MESSAGE

echo $CHANGES_SINCE_LAST_BUILD

file="./env.txt"

if [ -f "$file" ]
then
	echo "$file found."
    
    COMMIT_MESSAGE=$(cat "$file")
    echo $COMMIT_MESSAGE
    
    cat $COMMIT_MESSAGE

else
	echo "$file not found."
    
    touch $file
	echo $COMMIT_MESSAGE>>$file
fi