#the sessionfpg.pig is changed , so the dataPreparetion is of no need
import sys
data = sys.stdin.readlines()
lines = len(open("frequentpatterns",'rU').readlines());
countLine = 0;
for line in data:
    countLine = countLine+1;
    if countLine<=3 or countLine==lines:
         continue;

    parts = line.split(':')
    column1 = parts[0][0:];

    key = parts[1][0:];
    column2 = parts[2][0:];
    value = parts[3][0:];

    values = value.split("),");
    for v in values:
        v1 = v[0:];
        v1 = v1.replace("(","").replace(")","").replace("[","").replace("]","").replace(" ","");
#        print v1;        

        v2 = v1.split(",");
#        print v2;
        v3 =  v2[0:-1];
        for v4 in values:
            v4 = v4.replace("(","").replace(")","").replace("[","").replace("]","").replace(" ","");
#            print v4;
            v5 = v4.split(",");
#            print v5;
            v6 = v5[0:-1];
            v36 = [val for val in v6 if val in v3];
            array = [[] for i in range(3)]
#            if(len(v36)==len(v3)-1)and(len(v6)==len(v36)):
            if(len(v6)==len(v36) and len(v36)<len(v3)):
                array[0] = v6;
                array[1] = [val for val in v3 if val not in v6];
                array[2] = float(v2[-1])/float(v5[-1]);

                arr0 = tuple(array[0]);
                arr1 = tuple(array[1]);
                arr2 = tuple(str(array[2]));
                sys.stdout.write("put 'sporequest','");
                sys.stdout.write(key.replace(" ",""));
                sys.stdout.write("','arl:");
                for i in range(len(arr0)):
                    sys.stdout.write (arr0[i].replace(" ",""));
                    if (i!=len(arr0)-1):
                        sys.stdout.write(",");
                sys.stdout.write("=>");
                for i in range(len(arr1)):
                    sys.stdout.write(arr1[i].replace(" ",""));
                    if (i!=len(arr1)-1):
                        sys.stdout.write(",");
                sys.stdout.write("_confidence','");
#                print (array[2]),
#tuple has no atrribute of replace
#                sys.stdout.write(arr2.replace(" ",""));
#file object has no attribute of replace
#                sys.stdout.wirte(str(array[2]).replace(" ",""));
#                print(array[2]),
#                sys.stdout.write(array[2]);
                for i in range(len(arr2)):
                    sys.stdout.write(arr2[i].replace(" ",""));
#                    if (i!=len(arr2)-1):
#                        sys.stdout.write(",");
                print "'";
               
                sys.stdout.write("put 'sporequest','");
                sys.stdout.write(key.replace(" ",""));
                sys.stdout.write("','arl:");
                for i in range(len(arr0)):
                    sys.stdout.write (arr0[i].replace(" ",""));
                    if (i!=len(arr0)-1):
                        sys.stdout.write(",");
                sys.stdout.write("=>");
                for i in range(len(arr1)):
                    sys.stdout.write(arr1[i].replace(" ",""));
                    if (i!=len(arr1)-1):
                        sys.stdout.write(",");
                sys.stdout.write("_support','");
                arrs = tuple(v5[-1]);
                for i in range(len(arrs)):
                    sys.stdout.write(arrs[i].replace(" ",""));
#                    if (i!=len(arrs)-1):
#                        sys.stdout.write(",");
#                print (v5[-1]),
                print "'";

