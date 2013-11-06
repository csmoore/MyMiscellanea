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
# Filename: DeleteCompileFiles
# Requirements: Only works on Windows (uses Windows Delete command)
# Usage:   python DeleteCompileFiles.py <Directory> > output.txt
# WARNING: This will delete files matching "patternsToMatch" string list
#          Make sure you really, really want to do this.
#------------------------------------------------------------------------------

import sys
import os
import fnmatch

total_size = 0

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
                
def DoForEachFileInDirectory(root='', recurse=True, patterns = ["*.*"]):
    # ex. patterns = ["*.class", ]
    for pattern in patterns:
        for fspec in Walk(root, recurse, pattern):
            Execute_Command(fspec)

def Execute_Command(fspec):
    global total_size
    
    currentSize= os.path.getsize(fspec)
    total_size += currentSize 
    print "Deleting file: " + str(fspec) + ", size= " +str(currentSize)
    
    # Sample DOS Command from Python:
    # put file in quotes for command so filenames with spaces don't mess it up
    command = "del /q \"" + fspec + "\""
    os.system(command)       
          
def main():

    global total_size
    
    dirName=sys.argv[1]
    numberOfArguments=len(sys.argv)
    if numberOfArguments < 2:
        print("directory name expected. Usage: python ExecuteCommandDir.py <Directory Name>") 
        sys.exit()
    
    # *.sdf *.suo *.tlog *.ali *.o *.pch *.obj *.sbr
    # *.bsc *.map *.pdb *.ilk *.idb *.ncb *.opt *.aps *.plg
    # *.tlh *.tli *.pdb *.idb *.class *.vshost.exe
    # *.pyo *.pyc
    patternsToMatch = [ "*.sdf", "*.suo", "*.tlog", "*.ali", "*.o", "*.pch", "*.obj", "*.sbr",
                        "*.bsc", "*.map *.pdb", "*.ilk", "*.idb", "*.ncb", "*.opt", "*.aps", "*.plg",
                        "*.tlh", "*.tli", "*.pdb", "*.idb", "*.class", "*.vshost.exe",
                        "*.pyo", "*.pyc"]

    print "Deleting all matching files in folder: " + dirName
    print "Matching patterns=" + str(patternsToMatch)

    DoForEachFileInDirectory(dirName, True, patternsToMatch)

    print "Total size deleted: " + str(total_size)
            
if __name__ == '__main__':
    main()

