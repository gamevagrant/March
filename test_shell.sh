#!/bin/sh

# Fetch argments defined in Jenkins.
echo $gitlabActionType
echo $gitlabUserName
echo $GIT_COMMIT

export COMMIT_MESSAGE=\"$(git log --format=oneline -n 1 $CIRCLE_SHA1)\"
echo $COMMIT_MESSAGE

echo $CHANGES_SINCE_LAST_BUILD

# Variable that will trigger asset bundle build in comment by one commit.
export BUILD_AB="BUILD_ASSET_BUNDLE"

# env.txt file indicates if there is any changes by last commit.
file="./env.txt"
touch $file

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
		
		rm $file && touch $file
		echo $COMMIT_MESSAGE>>$file
	fi

else
	echo "$file not found, generate it!"
    
    touch $file
	echo $COMMIT_MESSAGE>>$file
fi

if [[ "$COMMIT_MESSAGE" == *"$BUILD_AB"* ]]; then
    return 1
fi
return 0