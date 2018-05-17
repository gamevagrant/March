now=$(date +"%T")
echo "Publishing Start: $now"

APK_OUTPUT_DIR=$JENKINS_HOME/package

if [ $Apk == true ]; then
	echo "Publish android apk..."

	# Get latest apk in package folder.
	unset -v latest
	for file in "$APK_OUTPUT_DIR"/*; do
	  [[ $file -nt $latest ]] && latest=$file
	done
	echo "Latest apk is : $latest"

	# Copy latest release apk plus march.apk for publishing.
	cp -vf "$latest" "package/march.apk"
	cp -vf "$latest" "package/"
fi

if [ $AssetBundle == true ]; then
	echo "Publish asset bundles..."
	
	# Copy asset bundles to workspace.
	rm -rvf AssetBundles
	cp -rvf $JENKINS_HOME/AssetBundleServer/AssetBundles/ AssetBundles
fi

now=$(date +"%T")
echo "Publishing Complete: $now"
