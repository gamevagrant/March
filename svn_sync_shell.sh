#!/bin/sh

# Update XML from configuration.
svn co http://svn.xinggeq.com/svn/march/doc/xml/client ./xml --force

# Copy configurations from ./xml to ./March
#echo "Workspace = $WORKSPACE"
echo "cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml"
cp -f -v ./xml/*.xml ./Assets/AssetBundleResources/xml

# Commit xmls to current repo.
git add .
git commit -m SVN_SYNC
git push origin HEAD:svn_sync
