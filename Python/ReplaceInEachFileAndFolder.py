#------------------------------------------------------------------------------
# Licensed under the Apache License, Version 2.0 (the "License");
# you may not use this file except in compliance with the License.
# You may obtain a copy of the License at
#
#   http://www.apache.org/licenses/LICENSE-2.0
#
# Unless required by applicable law or agreed to in writing, software
# distributed under the License is distributed on an "AS IS" BASIS,
# WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
# See the License for the specific language governing permissions and
# limitations under the License.
#------------------------------------------------------------------------------
# Filename: ReplaceInEachFileAndFolder.py
# Usage:    python ReplaceInEachFileAndFolder.py <Directory> > output.txt
# Requirement: must be text (non-binary) files
# WARNING: This will overwrite files matching "patternsToMatch" string with stringsToReplace
#          Make sure you really, really want to do this.
#------------------------------------------------------------------------------

import sys
import os
import fnmatch

# IMPORTANT: You *MUST* set these customized values (below are just examples)
file_patterns = ['*.svg']
stringsToReplace = \
    { \
        # "{OLD STRING}" : "{NEW STRING}", \ 
        'stroke-width="5"' : 'stroke-width="10"', \
        'stroke-width="4"' : 'stroke-width="8"', \
        'stroke-width="6"' : 'stroke-width="9"', \
        'font-family="sans-serif"' : 'font-family="\'Arial\'"', \
        'font-family="\'ArialMT\'"' : 'font-family="\'Arial\'"' \
    }

# Other Globals:
total_files_replaced = 0

# Walk credit: Robin Parmar: http://code.activestate.com/recipes/527746-line-of-code-counter/
def Walk(root='.', recurse=True, pattern='*'):
    """
        Generator for walking a directory tree.
        Starts at specified root folder, returning files
        that match our pattern. Optionally will also
        recurse through sub-folders.
    """
    for path, subdirs, files in os.walk(root):
        for name in files:
            if fnmatch.fnmatch(name, pattern):
                yield os.path.join(path, name)
        if not recurse:
            break
                
def DoForEachFileInDirectory(root='', recurse=True, patterns = ['*.*']):
    # ex. patterns = ["*.xxx", ]
    for pattern in patterns:
        for fspec in Walk(root, recurse, pattern):
            Execute_Command_On_File(fspec)

def Execute_Command_On_File(fspec):
    global total_files_replaced
    global stringsToReplace
    
    fileUpdated = False;

    # credit/taken/adapted from: http://stackoverflow.com/a/4128194
    # Read in old file
    with open(fspec,'r') as f:
        newlines = []
        for line in f.readlines():
            for stringToReplace in stringsToReplace :
                newString = stringsToReplace[stringToReplace]
                if (stringToReplace in line) :
                    fileUpdated = True
                line = line.replace(stringToReplace, newString)
            newlines.append(line)

    # Write changes to same file
    if fileUpdated :
        print("String Found and File Updated: " + fspec)
        total_files_replaced = total_files_replaced + 1
        with open(fspec, 'w') as f:
            for line in newlines:
                f.write(line)    
          
def main():

    global total_files_replaced
    global file_patterns
    
    numberOfArguments=len(sys.argv)
    if numberOfArguments > 1:
        dirName=sys.argv[1]
    else :
        print("Directory/Folder name expected. Usage: python ReplaceInEachFileAndFolder.py <Directory Name>") 
        dirName = "{TESTDIR}" # set this and comment out next line for standalone testing
        sys.exit()
    
    print("Replacing all matching strings in folder: " + dirName + " with file patterns: " + str(file_patterns))

    DoForEachFileInDirectory(dirName, True, file_patterns)

    print("Total Files Replaced/Updated: " + str(total_files_replaced))
            
if __name__ == '__main__':
    main()
