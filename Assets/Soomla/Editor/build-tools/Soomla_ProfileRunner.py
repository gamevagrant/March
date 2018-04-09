#!/usr/bin/python

from mod_pbxproj import *
from os import path, listdir
from shutil import copytree
import sys
import xml.etree.ElementTree as ET
import plistlib

script_dir = os.path.dirname(sys.argv[0])
build_path = sys.argv[1]
meta_data = sys.argv[2]

print ("script_dir:{0}".format(script_dir))
print ("build_path:{0}".format(build_path))
print ("metadata:{0}".format(meta_data))

frameworks = [
              'System/Library/Frameworks/Security.framework'
              ]

google_frameworks = [
                     'System/Library/Frameworks/AddressBook.framework',
                     'System/Library/Frameworks/AssetsLibrary.framework',
                     'System/Library/Frameworks/Foundation.framework',
                     'System/Library/Frameworks/CoreLocation.framework',
                     'System/Library/Frameworks/CoreMotion.framework',
                     'System/Library/Frameworks/CoreGraphics.framework',
                     'System/Library/Frameworks/CoreText.framework',
                     'System/Library/Frameworks/MediaPlayer.framework',
                     'System/Library/Frameworks/SystemConfiguration.framework',
                     'System/Library/Frameworks/UIKit.framework'
                    ]

google_gpgs_frameworks = [
                     'System/Library/Frameworks/CoreData.framework',
                     'System/Library/Frameworks/CoreTelephony.framework',
                     'System/Library/Frameworks/QuartzCore.framework',
                     'System/Library/Frameworks/Security.framework',
                     'usr/lib/libc++.dylib',
                     'usr/lib/libz.dylib'
                    ]

twitter_frameworks = [
                     'System/Library/Frameworks/Twitter.framework',
                     'System/Library/Frameworks/Social.framework',
                     'System/Library/Frameworks/Accounts.framework'
                    ]

weak_frameworks = [

]

itunes_app_id = ""
using_google_sdk = False
google_bundle_id = ""
google_client_id = ""
google_enable_gpgs = False
using_twitter_sdk = False
twitter_consumer_key = ""
using_gamecenter_sdk = False

parsed_metadata = meta_data.split('~')
itunes_app_id = parsed_metadata[0]

social_platform_data = parsed_metadata[1].split(';')
for social_platform in social_platform_data:
    parsed_social_platform = social_platform.split('^')
    if parsed_social_platform[0] == "twitter":
        using_twitter_sdk = True
        twitter_consumer_key = parsed_social_platform[1]
    elif parsed_social_platform[0] == "google":
        using_google_sdk = True
        google_bundle_id = parsed_social_platform[1]
        google_enable_gpgs = parsed_social_platform[2]
        google_client_id = parsed_social_platform[3]
    elif parsed_social_platform[0] == "gameCenter":
        using_gamecenter_sdk = True

print ("echo {0} {1} {2} {3} {4}".format(using_google_sdk, google_bundle_id, using_twitter_sdk, twitter_consumer_key, using_gamecenter_sdk))

pbx_file_path = build_path + '/Unity-iPhone.xcodeproj/project.pbxproj'
pbx_object = XcodeProject.Load(pbx_file_path)

for framework in frameworks:
    pbx_object.add_file_if_doesnt_exist(framework, tree='SDKROOT')

if using_google_sdk:
    for framework in google_frameworks:
        pbx_object.add_file_if_doesnt_exist(framework, tree='SDKROOT')
    if google_enable_gpgs:
        for framework in google_gpgs_frameworks:
            pbx_object.add_file_if_doesnt_exist(framework, tree='SDKROOT')
    # hopefully build_tools/../../../[Soomla]/Assets/Plugins/iOS
    google_framework_dir = path.join(script_dir,'..','..','..','WebPlayerTemplates','SoomlaConfig','ios', 'ios-profile-google', 'sdk')
    target_google_framework_dir = path.join(build_path, 'Libraries', 'ios-profile-google')
    copytree(google_framework_dir, target_google_framework_dir)
    pbx_object.add_framework_search_paths([path.abspath(target_google_framework_dir)])
    pbx_object.add_header_search_paths([path.abspath(target_google_framework_dir)])

    google_signin_framework = path.join(target_google_framework_dir, 'GoogleSignIn.framework')
    pbx_object.add_file_if_doesnt_exist(path.abspath(google_signin_framework), tree='SDKROOT')
    google_signin_framework_bundle = path.join(target_google_framework_dir, 'GoogleSignIn.bundle')
    pbx_object.add_file_if_doesnt_exist(path.abspath(google_signin_framework_bundle))

    google_framework = path.join(target_google_framework_dir, 'GooglePlus.framework')
    pbx_object.add_file_if_doesnt_exist(path.abspath(google_framework), tree='SDKROOT')
    
    google_framework_open = path.join(target_google_framework_dir, 'GoogleOpenSource.framework')
    pbx_object.add_file_if_doesnt_exist(path.abspath(google_framework_open), tree='SDKROOT')
    
    google_resource_bundle = path.join(target_google_framework_dir, 'GooglePlus.bundle')
    pbx_object.add_file_if_doesnt_exist(path.abspath(google_resource_bundle))

    if google_enable_gpgs:
        gpg_framework = path.join(target_google_framework_dir, 'gpg.framework')
        pbx_object.add_file_if_doesnt_exist(path.abspath(gpg_framework), tree='SDKROOT')
        gpg_framework_bundle = path.join(target_google_framework_dir, 'gpg.bundle')
        pbx_object.add_file_if_doesnt_exist(path.abspath(gpg_framework_bundle))

if using_twitter_sdk:
    for framework in twitter_frameworks:
        pbx_object.add_file_if_doesnt_exist(framework, tree='SDKROOT')

for framework in weak_frameworks:
    pbx_object.add_file_if_doesnt_exist(framework, tree='SDKROOT', weak=True)

pbx_object.add_other_ldflags('-ObjC')

pbx_object.save()

plist_data = plistlib.readPlist(os.path.join(build_path, 'Info.plist'))

plist_data["iTunesAppID"] = itunes_app_id

plist_types_arr = plist_data.get("CFBundleURLTypes")
if plist_types_arr == None:
    plist_types_arr = []
    plist_data["CFBundleURLTypes"] = plist_types_arr

if using_twitter_sdk:
    twitter_schemes = { "CFBundleURLSchemes" : ["tw{0}".format(twitter_consumer_key)]}
    plist_types_arr.append(twitter_schemes);

if using_google_sdk:
    google_url_schemes = [google_bundle_id]
    if google_enable_gpgs:
        reversed_google_key = '.'.join(list(reversed(google_client_id.split('.'))))
        google_url_schemes.append(reversed_google_key)
    google_schemes = { "CFBundleURLSchemes" : google_url_schemes, "CFBundleURLName" : google_bundle_id }
    plist_types_arr.append(google_schemes);

device_capabilities = plist_data.get("UIRequiredDeviceCapabilities")
if device_capabilities == None:
    device_capabilities = []
    plist_data["UIRequiredDeviceCapabilities"] = device_capabilities

if using_gamecenter_sdk:
    device_capabilities.append("gamekit")

plistlib.writePlist(plist_data, os.path.join(build_path, 'Info.plist'))
