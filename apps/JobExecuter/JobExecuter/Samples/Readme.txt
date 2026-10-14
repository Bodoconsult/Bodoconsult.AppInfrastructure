**********************************
H1 Advanced features
**********************************

**********************************
H2 Using Readme.txt
**********************************

If in the directory of a JobExecuter job - there where the JSON file of job is located - is Readme.txt placed, this file will be included in the job's documentation.

***************
H3 Tags usable in the Readme.txt
***************

In such a Readme.txt you can use the following tags to format the readme.txt if it is processed to the job documentation. The following tags are interpreted:

DL H1 DD Header level 1
DL H2 DD Header level 2
DL H3 DD Header level 3
DL DL DD Definition list (must be followed by a DD tag)
DL LI DD List items

All tags will be finished at the next carriage return+line feed (char(13)+char(10)).

***************
H3 Comments in Readme.txt
***************

Comments made of asterikses (*) will be removed when formatting Readme.txt to plain text or HTML

***************
H3 Sample
***************

The HTML result of processing such a Readme.txt you can find in the file Readme.htm