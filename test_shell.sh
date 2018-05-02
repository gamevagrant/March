#!/bin/sh

# Fetch argments defined in Jenkins.
echo $gitlabActionType
echo $gitlabUserName
echo $GIT_COMMIT

export COMMIT_MESSAGE=\"$(git log --format=oneline -n 1 $CIRCLE_SHA1)\"
echo $COMMIT_MESSAGE

echo $CHANGES_SINCE_LAST_BUILD

# Variables file to env.txt
file="./env.txt"

if [ -f "$file" ]
then
	echo "$file found."
    
    export LAST_COMMIT=$(cat "$file")
    echo $LAST_COMMIT
	if [ "$LAST_COMMIT" == "$COMMIT_MESSAGE" ]
	then
		echo "No change."
	else
		echo "Changed"
	fi

else
	echo "$file not found."
    
    touch $file
	echo $COMMIT_MESSAGE>>$file
fi