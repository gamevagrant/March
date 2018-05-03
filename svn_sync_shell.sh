#!/bin/sh

# Update XML from configuration.
rm -f ./xml
svn co http://svn.xinggeq.com/svn/march/doc/xml/client ./xml

# Copy configurations from ./xml to ./March
#echo "Workspace = $WORKSPACE"
echo "cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml"
cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml

# Commit xmls to current repo.
git add .
git commit -m SVN_SYNC
git push