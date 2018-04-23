#!/bin/sh

now=$(date +"%T")
echo "Build Start: $now"

# Android Stuff.
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

# Check apk output dir.
if [ "$APK_OUTPUT_DIR" == "" ]
then
	echo $APK_OUTPUT_DIR is undefined.
	exit 0
else
	echo output dir is $APK_OUTPUT_DIR.
fi

# Unity batch mode build.
echo "Compiling.. this will take a while"
echo $untiy -quit -batchmode -projectPath $projectPath -logFile `pwd`/editor.log -executeMethod ProjectBuild.JenkinsBuildAndroid
$untiy -quit -batchmode -projectPath $projectPath -logFile `pwd`/editor.log -executeMethod ProjectBuild.JenkinsBuildAndroid

now=$(date +"%T")
echo "Build Complete: $now"