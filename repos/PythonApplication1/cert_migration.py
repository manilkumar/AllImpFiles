#!/usr/bin/python
import os
import uuid
import sys
from azure.storage.blob import BlobClient
import glob
import datetime
from time import time as _time

def get_old_dirs(dir_path, older_than_days):
    time_now = _time()
    dirs = []
    for path, folders, files in os.walk(dir_path):
        for folder in folders:
            folder_path = os.path.join(path, folder)
            if (time_now - os.path.getmtime(folder_path)) < older_than_days:
                dirs.append(folder_path)
    return dirs

def migrate_certs(source_path,folders_list,conn_string):
    try:
        if source_path == '':
            print("" + str(datetime.datetime.now()) + " Total no of certs to be migrated: " + str(len(folders_list)) + "")
            for dir in folders_list:
                for r,d,f in os.walk(dir):        
                    if f:
                        for file in f:
                            blob = BlobClient.from_connection_string(conn_string, container_name="certificate", blob_name=(r[-32:] + ".pdf"))
                            file_path_on_local = os.path.join(r,file)
                            print("migarting file: " + r + ".pdf")
                            # Upload the certifcate folder
                            with open(file_path_on_local, "rb") as data:
                                blob.upload_blob(data,overwrite=True)
        else:
            print("" + str(datetime.datetime.now()) + " Total no of certs to be migrated: " + str(len(next(os.walk(source_path))[1])) + "")
            for r,d,f in os.walk(source_path):        
                   if f:
                      for file in f:
                          blob = BlobClient.from_connection_string(conn_string, container_name="certificate",blob_name=(r[-32:] + ".pdf"))
                          file_path_on_local = os.path.join(r,file)
                          print("migarting file: " + file + "")
                              # Upload the certifcate folder
                          with open(file_path_on_local, "rb") as data:
                              blob.upload_blob(data,overwrite=True)
        print("Migrated successfully !")
    except Exception as e:
        print(e)


# Main method.
if __name__ == '__main__':
    source_path = sys.argv[1]
    conn_string = "DefaultEndpointsProtocol=https;AccountName=uatmedust;AccountKey=CtDruDF6fwwYUG2lvSSvDkDhUBvfeHoqOQlMHpw5vPv63RzdVBtlPjZAq1MixvevPU93dW6pawnwjA+hde9h2Q==;EndpointSuffix=core.windows.net"
    days = 2
    if(sys.argv[2] == 'true'):
       migrate_certs(source_path,"",conn_string)
    else:
       list_of_folders = get_old_dirs(source_path,60 * 60 * 24 * days)
       migrate_certs("",list_of_folders,conn_string)
