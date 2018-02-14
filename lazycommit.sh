#!/bin/bash/

echo "Type in commit message: "
read str

git add -A
git commit -m "'$str'"
git push 
