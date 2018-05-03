#!/bin/sh

# Update XML from configuration.
svn co http://svn.xinggeq.com/svn/march/doc/xml/client ./xml

# Copy configurations from ./xml to ./March
#echo "Workspace = $WORKSPACE"
echo "cp -f -v ../xml/*.xml ./Assets/StreamingAssets/xml"
cp -f -v ./xml/*.xml ./Assets/StreamingAssets/xml

now=$(date +"%T")
echo "Build Start: $now"

# Build parameters.
export ProductName=March
export CompanyName=elex
export ApplicationId=com.elex.march

export ANDROID_KEYSTORE_NAME=user.keystore
export ANDROID_KEYSTORE_PASSWORD=111111
export ANDROID_KEYALIAS_NAME=test
export ANDROID_KEYALIAS_PASSWORD=111111
export ANDROID_SDK_ROOT=

# Build folder structure.
export APK_OUTPUT_DIR=package

# Unity app path.
export untiy=/Applications/Unity_2017.2.0f3/Unity.app/Contents/MacOS/Unity

# Project path.
export projectPath=`pwd`

# Fetch argments defined in Jenkins.
echo "version = $Version"
echo "IsForDev = $IsForDev"
echo "output dir is $APK_OUTPUT_DIR."

# Get git commit comment.
export COMMIT_MESSAGE=\"$(git log --format=oneline -n 1 $CIRCLE_SHA1)\"
export BUILD_AB_COMMIT="build ab"

# Unity batch mode build.
echo "Compiling.. this will take a while"
echo $untiy -quit -batchmode -projectPath $projectPath -logFile `pwd`/editor.log -executeMethod ProjectBuild.JenkinsBuildAndroid
$untiy -quit -batchmode -projectPath $projectPath -logFile `pwd`/editor.log -executeMethod ProjectBuild.JenkinsBuildAndroid

now=$(date +"%T")
echo "Build Complete: $now"