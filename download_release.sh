#!/usr/bin/env bash

# remember to export in godot

rm -r downloaded_release
mkdir -v downloaded_release

mardl BallDuelRelease.zip
mv BallDuelRelease.zip downloaded_release

(cd downloaded_release && unzip BallDuelRelease.zip)

echo unzipped in downloaded_release/release

