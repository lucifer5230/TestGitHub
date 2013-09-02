Register '/home/hadoop/DM/myUDF.py' using jython as myfuncs;
Register '/home/hadoop/DM/myUDF_ly.py' using jython as myfuncs_ly;
Path0 = load 'hdfs://v2namenode:8020/spo/requestcommon' using PigStorage('\t') as (timestampUtc:chararray, userLogin:chararray, documentPath:chararray, referrerUrl:chararray, requestType:chararray, duration:double, serverUrl:chararray, browser:chararray, httpStatus:chararray);
Path = filter Path0 by (userLogin matches '.*@.*') AND (NOT documentPath matches '.*getsitecollectionservice.asmx.*') AND (NOT documentPath matches '.*default.*') AND (NOT documentPath matches '/') AND (NOT documentPath matches '.*authenticate.*') AND (NOT documentPath matches '.*PII.*') AND (NOT documentPath matches '.*redirect.*') AND (NOT documentPath matches '.*blank.*') AND (NOT documentPath matches '.*sitecollections.*') AND (NOT documentPath matches '.*picker.*') AND (NOT documentPath matches '.*viewproperties.*') AND (NOT documentPath matches '.*picker.*') AND (NOT documentPath matches '.*accessdenied.*')AND (NOT documentPath matches '.*manageuserprofileserviceapplication.*') AND (NOT documentPath matches '.*ta_searchadministration.*') AND (NOT documentPath matches '.*ta_bcshome.*') AND (NOT documentPath matches '.*ta_managesssvcapplication.*') AND (NOT documentPath matches '.*tenantadminapps.*');

Path1 = foreach Path generate userLogin,myfuncs_ly.pagename_ly(documentPath) as doc, timestampUtc;
/*
Path1 = foreach Path generate userLogin,documentPath, timestampUtc;
*/
Path2 = group Path1 by userLogin;
Path3 = foreach Path2 {
D = order Path1 by timestampUtc;
generate group, D;
};
Store Path3 INTO 'hdfs://v2namenode:8020/spo/tmp/session' using PigStorage('\t');

