now=$(date +"%T")
echo "Publishing Start: $now"

APK_OUTPUT_DIR=$JENKINS_HOME/package

# Get latest apk in package folder.
unset -v latest
for file in "$APK_OUTPUT_DIR"/*; do
  [[ $file -nt $latest ]] && latest=$file
done
echo "Latest apk is : $latest"

# Copy latest release apk plus march.apk for publishing.
echo cp -vf "$latest" "package/march.apk"
cp -vf "$latest" "package/march.apk"
cp -vf "$latest" "package/"

# Copy asset bundles to workspace.
echo rm -rvf AssetBundles
rm -rvf AssetBundles
echo cp -rvf $JENKINS_HOME/AssetBundleServer/AssetBundles/ AssetBundles
cp -rvf $JENKINS_HOME/AssetBundleServer/AssetBundles/ AssetBundles

now=$(date +"%T")
echo "Publishing Complete: $now"
