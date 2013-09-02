Register '/home/hadoop/DM/myUDF.py' using jython as myfuncs;
User = load 'hbase://spouser' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('profile:firstlogin', '-loadKey true') as (user:chararray, week:chararray);
WeekGroup = group User by week;
Register '/home/hadoop/DM/myUDF.py' using jython as myfuncs;
User = load 'hbase://spouser' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('profile:firstlogin', '-loadKey true') as (user:chararray, week:chararray);
WeekGroup = group User by week;
userCount = foreach WeekGroup generate group as week, COUNT(User);
STORE userCount INTO 'hbase://sposession' USING org.apache.pig.backend.hadoop.hbase.HBaseStorage('data:usercount');
Path0 = load 'hdfs://v2namenode:8020/spo/request/requestcommon' using PigStorage('\t') as (timestampUtc:chararray, userLogin:chararray, documentPath:chararray, referrerUrl:chararray, requestType:chararray, duration:double);
Path = filter Path0 by (userLogin matches '.*@.*') AND (NOT documentPath matches '.*getsitecollectionservice.asmx.*');
Path2 = foreach Path generate userLogin, timestampUtc, myfuncs.week(timestampUtc) as week;
Path4 = group Path2 by (userLogin, week);
Path5 = foreach Path4
{
        D = distinct Path2.timestampUtc;
        Generate group.userLogin, group.week, myfuncs.sessionCount(D) as count, myfuncs.sessionLength(D) as length;
 };
Path6 = join Path5 by userLogin, User by user;
Path7= group Path6 by (User::week, Path5::week);
Path8 = foreach Path7 generate group, COUNT(Path6), myfuncs.avgwithoutzero(Path6.length);

store Path8 into 'hdfs://v2namenode:8020/spo/tmp/sposession' using PigStorage(',');
