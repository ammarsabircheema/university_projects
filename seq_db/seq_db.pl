    use strict;
    use warnings;
    use diagnostics;
    use DBI;


    #  Declaring Variables
    my ($sql, %feilds, @keys, @vals,$seqID,$q);


          my @order =qw(ID first_name last_name  initials email  tel_number mob_number institute_name 
          lab_name adress street city province country region other_author_names reference personal_statements 
          comments  sequence_id sequence_file );


           my %fields =(
		'first_name'     => "First Name: "   ,
                'last_name' => "Last name: " , 
                'initials'  => "Intials: "   ,
                'email' => "Email: " , 
                'tel_number' => "Telephone Number: ",
                'mob_number'=> "Mobile Number: ",
                'institute_name' => "Institute: " , 
                'lab_name' => "Lab: " ,
                'adress'=> "Address: ",
                'street'=> "Street: " ,
                'city'=> "City: ",
                'country'=> "Country: ",
                'province'=> "Province: ",
                'region'=> "Region: " ,
                'other_author_names'=> "Other Authors: ",
                'reference'=> "Reference: ", 
                'personal_statements'=>"Personal Statements: ", 
                'comments'=>"Comments: ",
                'sequence_id'=>"Sequence ID: ",
                'sequence_file'=>"Sequence file: "
				);
            
            
    #For Connecting with database                
    my $conn = DBI->connect("DBI:mysql:pcr_experiment","root","password15")
       or die("Cannot connect: $DBI::errstr");

                                    
                                    
    do
	{
		print "Enter","\n";
		print "1.To enter Record","\n";
		print "2.To view Record","\n";
		print "3.To Delete Record","\n";
		print "4.To Quit","\n";
		$q=<STDIN>;
		 if($q==1)
		  {
            Get_Data(); 
		  }
          elsif($q==2)
		  {
            View_Record(Get_ID());
		  }
				elsif($q==3)
          {
          Delete_Record(Get_ID());
          }

    }until ($q =='4' );
    exit;


    sub Get_Data
    {
				  $fields{'first_name'} =Get_Input('first_name'    );
				  $fields{'last_name'} =Get_Input('last_name'    );
				  $fields{'initials'} =Get_Input('initials' );
				  $fields{'email'} =Get_Input('email'  );
				  $fields{'tel_number'} =Get_Input('tel_number'   );
				  $fields{'mob_number'} =Get_Input('mob_number');
				  $fields{'institute_name'} =Get_Input('institute_name'  );
				  $fields{'lab_name'} =Get_Input('lab_name'  );  
				  $fields{'adress'} =Get_Input('adress' );
				  $fields{'street'} =Get_Input('street' );
				  $fields{'city'} =Get_Input('city' );
				  $fields{'province'} =Get_Input('province');
				  $fields{'country'} =Get_Input('country');
				  $fields{'region'} =Get_Input('region');
				  $fields{'other_author_names'} =Get_Input('other_author_names');
				  $fields{'reference'}=Get_Input( 'reference');
				  $fields{'personal_statements'} =Get_Input('personal_statements');
				  $fields{'comments'} =Get_Input('comments');
				  $fields{'sequence_id'} =getseqid();
				  $fields{'sequence_file'} =seqfile();
                  Execute_Transaction();
    }

    sub Get_Input 
    {
				  print $_[0], ":\n";
				  return scalar <STDIN>;
    }



    sub Execute_Transaction
    {
				 @keys = keys   %fields;
				 @vals = values %fields;

				 chomp(@vals);

				 @vals = map{$conn->quote($_)} @vals;
				 $sql = "INSERT INTO UNIVERSITY("
						 . join(", ", @keys)
						 . ") VALUES ("
						 . join(", ", @vals)
						 . ")";

				 my $query = $conn->prepare($sql);

				 $query->execute 
				 or die("\nError executing SQL statement! $DBI::errstr");

				 print "Record added to database.\n";

				 return 1;
    }


    sub Get_ID
    {
				print "Enter ID #:\n";

				$seqID = <STDIN>;
				chomp($seqID);

				return($seqID);
    }


    sub Delete_Record
    {
                        
				my $result = $conn->do("DELETE FROM university WHERE sequence_id = '$seqID'")
					or die("\nError executing SQL statement! $DBI::errstr");

         if($result)
		{
        print "Record deleted from database.\n";
        }
        else 
        {
        print "Record NOT DELETED! $DBI::errstr\n";
        }

        return;
        }


    sub View_Record
    {



			   my $sql = qq(SELECT * FROM university WHERE sequence_id = '$seqID');   
			   my $sth = $conn->prepare($sql);
			   $sth->execute;
          while(my $record = $sth->fetch)
       {
                       my $i=0;
           for my $field (@$record)
	       {  
           print "$order[$i]",":","$field\n"; 
           $i++;
           }
       }
                       print "\n\n";
                       return;
    }



    sub seqfile
    {
			   my $option;
			   my $sek;
			   print "1.To enter Sequence Manually","\n";
			   print "2.To enter file","\n";
			   $option=<STDIN>;
                       
					   if($option==1)
		{
			   print "Enter Sequence","\n"; 
			   $sek=<STDIN>;
			   return $sek;
 
		}
                       elsif($option==2)
		{
				print "Enter file name","\n"; 
				my $proteinfilename = <STDIN>;#For input in fn
				chomp $proteinfilename;
				open(FILE, $proteinfilename);#PROTEINFILE IS FILE HANDLER
     

			unless ( open(FILE, $proteinfilename) )#This statement checks whether file is present or not
			{

			     print "\n";
				 print "Cannot open file \"$proteinfilename\"\n\n";
				 exit;
			}
                        
				my @protein=<FILE>;#Saving file in protein array
				close FILE;#Closing file.Erases file in memory
				my $protein = join( '', @protein);#To remove spaces

				$protein=~ s/\s//g;#To remove all spaces
				return $protein;
		}
    }


    sub getseqid
      {
				open(MYFILE,"value.txt"); # Open the fasta file
				my $idgenerator = <MYFILE>; # Read the entire file into an array
				close MYFILE; # Close the file
				my $var;
				$idgenerator++;
				(my $p)=substr("$fields{'province'}",0,2);
				(my $c)=substr("$fields{'country'}",0,2);   
				(my $r)=substr("$fields{'region'}",0,1);
				my @string = $idgenerator;

			for(my $i=1000000; $i-1>$idgenerator; )
            {
			$i= $i/10;
			unshift ( @string , '0');
            }


            $var=$r . $c . $p . join('',@string);


			   open(HFILE,">value.txt"); # Open the file for writing
			   print HFILE $idgenerator; # Put something in the file
			   close HFILE; 
			   return $var;
      }
