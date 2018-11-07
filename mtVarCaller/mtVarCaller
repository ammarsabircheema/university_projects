#!/usr/bin/python3
from __future__ import division
import pysam,argparse,sys,collections,subprocess,matplotlib.pyplot as plt
from collections import Counter

parser = argparse.ArgumentParser(description='mtVarCaller detects heteroplasmy from alignment files (bam format) of mitochondrial genomes. mtVarCaller works only for BAM format.',usage = 'python mtVarCaller.py -r REF_FILE -i BAM_FILE [options]')
parser.add_argument('-v', action='version', version='mtVarCaller 1.0')
parser.add_argument('-q',dest='base_qual' ,type = int,action='store',help='Base qualities',default = 30)
parser.add_argument('-mq',dest='mapping_qual' ,type = int,action='store',help='Mapping quality',default = 15)
parser.add_argument('-nb',dest='no_of_bases' ,type = int,action='store',help='Number of bases to be considered for a position',default = 2000)
requiredNamed = parser.add_argument_group('required arguments')
requiredNamed.add_argument('-r',dest='ref_file',required=True, type = str	,action='store',help='Reference File')
requiredNamed.add_argument('-i',dest='bam_file', required=True,action='store',type = str,help='Input File')
if len(sys.argv) == 1:
     parser.parse_args(['-h'])
else:
     args = parser.parse_args()

global ref_file  # Reference file which will be used as a reference  
ref_file = args.ref_file
global bamfile   # BAM file which is input file 
bamfile = args.bam_file
global base_qual  # Quality threshold below which bases will be ignored
base_qual = args.base_qual
global mapping_qual  # Mapping Quality threshold below which bases will be ignored
mapping_qual = args.mapping_qual
global no_of_bases  # Number of bases at a position below which the position will not be considered
no_of_bases = args.no_of_bases


 

############################################ Function for getting reference_bases #######################################################

def get_reference(ref_file):
   with open(ref_file,'r') as f:
      reflist = f.readlines()[1:]
      reflist = ''.join(reflist)
      reflist = reflist.replace('\n','')
      reflist = ','.join(reflist)
      reflist = reflist.split(',')
   reference_bases = ['A','T','G','C','N']
   if (len(reflist) < 16569)   or ([x for x in reflist if x not in reference_bases]):
      print("Invalid reference")
      sys.exit()  
   else:
      return reflist

############################################ Function for getting bases out of bam file #################################################

def getbases(filename,lenref):
   try:
      samfile = pysam.Samfile( filename, "rb" ) 
   except OSError as err:
      print("OS error: {0}".format(err))  
   ReverseList = [''] * lenref
   ForwardList = [''] * lenref
   X = []
   for pileupcolumn in samfile.pileup(max_depth = 1000000) :
      X.append(pileupcolumn.n)
      for pileupread in pileupcolumn.pileups:
         if (pileupread.alignment.mapping_quality <= mapping_qual):
                 continue      
         if not pileupread.is_del and not pileupread.is_refskip:
                 if pileupread.alignment.query_qualities[pileupread.query_position] < base_qual:
                 # Skip entries with base phred scores < base_qual
                      continue
                 if pileupread.alignment.is_reverse: #negative
                      ReverseList[pileupcolumn.pos] += pileupread.alignment.query_sequence[pileupread.query_position]
                 else:
                      ForwardList[pileupcolumn.pos] += pileupread.alignment.query_sequence[pileupread.query_position]

   Y = range(len(X))
   plt.title(bamfile)
   plt.xlabel('Bases')
   plt.ylabel('Coverage')
   plt.plot(Y, X,c = 'g')
   plt.grid(True, lw = 2, ls = '-', c = '.75')
   plt.xlim(0, 16569)
   avgl = plt.axhline(y=(sum(X)/len(X)), color='b', linestyle='--') # line for average
   cutoffl = plt.axhline(y=2000, color='r', linestyle='-') # line for cutoff
   plt.legend((avgl,cutoffl),('Average coverage','Cutoff'),loc='upper right',fancybox=True) # plotting legend for plot
   plt.savefig(bamfile+'_Coverage_plot.png', c = 'k')
   samfile.close() # Change 1
   return ForwardList,ReverseList            
   
   

############################################ Function for filtering reads ###############################################################

def FilterReads(in_file, out_file):

    def read_ok(read):
        """
        read_ok - reject reads with a low quality (<5) base call
        read - a PySam AlignedRead object
        returns: True if the read is ok
        """
        if any([ord(c)-33 < _BASE_QUAL_CUTOFF for c in list(read.qual)]):
            return False
        else:
            return True

    _BASE_QUAL_CUTOFF = 5

    bam_in = pysam.Samfile(in_file, 'rb')
    bam_out = pysam.Samfile(out_file, 'wb', template=bam_in)


    for read in bam_in.fetch():
        if read_ok(read):
            bam_out.write(read)



    bam_out.close()
    bam_in.close()


########################################## Function for getting major and minor bases ####################################################

def maj_base(word):
    c = collections.Counter(word)
    if len(c) == 0:
       return '-'
    return c.most_common(1)[0][0]

def min_base(word):
    c = Counter(word).most_common(4)
    if len(c) == 1 or len(c) == 0:
       return '-'
    elif len(c) == 2 :
       return c[1][0]
    elif ((len(c) == 3) or (len(c) == 4)) and (c[1][1] > c[2][1]) :
       return c[1][0]
    elif ((len(c) == 3) or (len(c) == 4 and c[3][1] < c[2][1])) and (c[1][1] == c[2][1]):
       if (c[1][0] + c[2][0] == 'AG') or (c[1][0] + c[2][0] == 'GA'):
          return 'R'     
       elif (c[1][0] + c[2][0] == 'AC') or (c[1][0] + c[2][0] == 'CA'):
          return 'M'     
       elif (c[1][0] + c[2][0] == 'CG') or (c[1][0] + c[2][0] == 'GC'):
          return 'S'     
       elif (c[1][0] + c[2][0] == 'CT') or (c[1][0] + c[2][0] == 'TC'):
          return 'Y'
       elif (c[1][0] + c[2][0] == 'GT') or (c[1][0] + c[2][0] == 'TG'):
          return 'K'
       elif (c[1][0] + c[2][0] == 'AT') or (c[1][0] + c[2][0] == 'TA'):
          return 'W'
    elif (len(c) == 4) and (c[1][1] == c[2][1] == c[3][1]):
       if 'A' not in (c[1][0] , c[2][0] , c[3][0] ):
          return 'B'
       if 'T' not in (c[1][0] , c[2][0] , c[3][0] ):
          return 'V'
       if 'G' not in (c[1][0] , c[2][0] , c[3][0] ):
          return 'H'
       if 'C' not in (c[1][0] , c[2][0] , c[3][0] ):
          return 'D'      
    else: 
       return '-'


########################################## Function for counting major and minor bases ###################################################

def count_maj(base,strand):
    base_count = strand.count(base)
    return base_count

def count_min(base,strand):
    if base == 'R':
        min_base_count = strand.count('A') + strand.count('G')
    elif base == 'M':
        min_base_count = strand.count('A') + strand.count('C')
    elif base == 'S':
        min_base_count = strand.count('C') + strand.count('G')
    elif base == 'Y':
        min_base_count = strand.count('C') + strand.count('T')
    elif base == 'K':
        min_base_count = strand.count('G') + strand.count('T')
    elif base == 'W':
        min_base_count = strand.count('A') + strand.count('T')
    elif base == 'B':
        min_base_count = strand.count('C') + strand.count('T') + strand.count('G')
    elif base == 'V':
        min_base_count = strand.count('C') + strand.count('A') + strand.count('G')
    elif base == 'H':
        min_base_count = strand.count('C') + strand.count('T') + strand.count('A')
    elif base == 'D':
        min_base_count = strand.count('A') + strand.count('T') + strand.count('G')
    else:
        min_base_count = strand.count(base)
    return min_base_count

########################################## Function to calculate heteroplasmy ############################################################

def calculate_heteroplasmy(ref,Fwd,Rev):
   Het = 0
   fout = open(bamfile+".vcf", 'w')
   fout.write("{}\n{}\n{}{}\n{}{}\n{}\n{}\n{}\n{}\t{}\t{}\t{}\t{}\t{}\t{}\t{}".format("##fileformat=VCFv4.0","##program=mtVarCaller","##source=",' '.join(sys.argv),"##reference=",ref_file,"##INFO=<ID=DP,Number=1,Type=Integer,Description=\"Coverage\">","##INFO=<ID=AF,Number=1,Type=Float,Description=\"Allele Frequency\">","##INFO=<ID=DP4,Number=4,Type=Integer,Description=\"Counts for ref-forward bases, ref-reverse, alt-forward and alt-reverse bases\">","#CHROM","POS","ID","REF","ALT","QUAL","FILTER","INFO"))
   for index,(i, r, f) in enumerate(zip(ref, Rev, Fwd)):   
       Total_min_base_count = count_min(min_base(f),f) + count_min(min_base(r),r)
       Total_fwd_base_count = count_maj(maj_base(f),f) + count_maj(maj_base(r),r)
       
       MAFREV = len(r) * 0.01
       
       MAFFWD = len(f) * 0.01
       
       if no_of_bases == 2000:    
           if ((Total_min_base_count + Total_fwd_base_count) == 0) or (count_min(min_base(r),r) < MAFREV) or (count_min(min_base(f),f) < MAFFWD) or (len(r) < 2000) or (len(f) < 2000) or (66 <= index <= 71) or ( 303 <= index <= 311) or ( 514 <= index <= 523 ) or ( 3105 <= index <= 3109) or ( 12418 <= index <= 12425) or ( 16184 <= index <= 16193):
               continue
       else:
           if ((Total_min_base_count + Total_fwd_base_count) == 0) or (count_min(min_base(r),r) < MAFREV) or (count_min(min_base(f),f) < MAFFWD) or (len(r) < 40) or (len(f) < 40) or (66 <= index <= 71) or ( 303 <= index <= 311) or ( 514 <= index <= 523 ) or ( 3105 <= index <= 3109) or ( 12418 <= index <= 12425) or ( 16184 <= index <= 16193):
               continue

       Het =  Total_min_base_count / (Total_min_base_count + Total_fwd_base_count)
       if min_base(f) == min_base(r): 
           fout.write("\n{}\t{}\t{}\t{}\t{}\t{}\t{}\t{}{}{}{}{:.4f}{}{}{}{}{}{}{}{}{}".format("ChrM",index+1,".",i,min_base(f),".","PASS","DP=",len(f)+len(r),';',"AF=",Het,';',"DP4=",count_maj(maj_base(f),f),",",count_min(min_base(f),f),",",count_maj(maj_base(r),r),",",count_min(min_base(r),r)))
       else:
           fout.write("\n{}\t{}\t{}\t{}\t{}{}{}\t{}\t{}\t{}{}{}{}{:.4f}{}{}{}{}{}{}{}{}{}".format("ChrM",index+1,".",i,min_base(f),",",min_base(r),".","PASS","DP=",len(f)+len(r),';',"AF=",Het,';',"DP4=",count_maj(maj_base(f),f),",",count_min(min_base(f),f),",",count_maj(maj_base(r),r),",",count_min(min_base(r),r)))         
   fout.close()  

########################################## Function to calculate homoplasmy ##############################################################

def calculate_homoplasmy(ref,Fwd,Rev):
    hout = open(bamfile+"hom.txt", 'w')
    hout.write("{}\t{}\t{}\t{}\t{}".format('Pos','rcrs','Alt','Fwdcov','Revcov'))
    for index,(i, r, f) in enumerate(zip(ref, Rev, Fwd)):
           if (len(r) < 40) or (len(f) < 40)  : # if coverage is <40 then reject that site
                 continue  
           if maj_base(r) != i:
                 hout.write("\n{}\t{}\t{}\t{}\t{}".format(index+1,i,maj_base(r),len(f),len(r)))
    hout.close()

########################################################   Program   #######################################################################


###Filtering the bam file

outfile = 'output.bam'
FilterReads(bamfile,outfile)
sortedfile = 'outfile.sorted.bam'
pysam.sort("-o",sortedfile,outfile)
pysam.index(sortedfile)

###Saving the reference in list and getting in form of reflist

reflist = get_reference(ref_file)

###Getting length of reference (Revised Cambridge Reference Sequence)

lenref = len(reflist) 

###Getting strand based coverage out of coverage file

ForwardList,ReverseList = getbases(sortedfile,lenref)

###Calculating Heteroplasmies

calculate_heteroplasmy(reflist,ForwardList,ReverseList)

###Calculating Homplasmies

calculate_homoplasmy(reflist,ForwardList,ReverseList)

##################################################### End of Program #######################################################################
