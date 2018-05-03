#!/bin/sh

# Update XML from configuration.
svn co http://svn.xinggeq.com/svn/march/doc/xml/client ./xml --force

# Copy configurations from ./xml to ./March
#echo "Workspace = $WORKSPACE"
echo "cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml"
cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml

# Commit xmls to current repo.
git add ./Assets/AssetBundleResources/xml/\*.xml
git commit -m "build ab since svn doc updated."
git push origin HEAD:$NODE_NAME
