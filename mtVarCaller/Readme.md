# Installation:

To install mtVarCaller run install.sh
```chmod +x install.sh
./install.sh
```
# Implementation

- After installation mtVarCaller can be used to analyze data. 
- mtVarCaller analyzes BAM files.
- For this purpose BAM file should be sorted and indexed. 
- Put BAM file, index of BAM file and reference in same directory as follows:

```mtVarCaller -r reference.fa -i sample.BAM```