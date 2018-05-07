#!/bin/sh

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

# Pre handle asset bundles.
if [[ $PredeineSymbols ~= *GAME_DEBUG* ]]; then
	# Remove asset bundle from streaming asset folder.
	AssetBundleStreamingRoot="$WORKSPACE/Assets/StreamingAssets/AssetBundles"
	if [ -d "$AssetBundleStreamingRoot" ]; then
		rm -rvf $AssetBundleStreamingRoot
	fi
fi

# Unity batch mode build.
echo "Compiling.. this will take a while, redirect log file to console..."
echo $untiy -quit -batchmode -projectPath $projectPath -logFile -executeMethod ProjectBuild.JenkinsBuildAndroid
$untiy -quit -batchmode -projectPath $projectPath -logFile -executeMethod ProjectBuild.JenkinsBuildAndroid

# Post handle asset bundles.
if [[ $PredeineSymbols ~= *GAME_DEBUG* ]]; then
	# copy asset bundles to web server.
	AssetBundleRoot="$WORKSPACE/AssetBundles"
	WebServerRoot="$JENKINS_HOME/AssetBundleServer/AssetBundles"
	if [ ! -d "$WebServerRoot" ]; then
		mkdir "$WebServerRoot"
	fi
	rm -rvf $WebServerRoot
	cp -rvf $AssetBundleRoot $WebServerRoot
fi

now=$(date +"%T")
echo "Build Complete: $now"