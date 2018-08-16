#!/usr/bin/env bash

###############################
#
# AMKDownloadManager
# This file provided to allow
# short access to application entry
# along with platform and runtime selection.
# 
# Last modified on 2018/08/12 (yyyy/mm/dd)
# by Ali Mousavi Kherad
#
###############################

if hash mono 2>/dev/null; then
    mono "./AMKDownloadManager.UI.Xamarin.GtkSharp.exe"
else
    wine "./AMKDownloadManager.UI.Xamarin.GtkSharp.exe"
fi