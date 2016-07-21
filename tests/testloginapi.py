
# test login api 0.1 - Warren Postma - requires python 3.5+
#
# This is a quick test script to check if HTTP post generates a token
# and then if the token that is generated will satisfy .net core auth
# middleware.
#
# if "import requests" fails type "pip3 install requests"
# to install library "requests".

import sys
import requests  # may not be installed on every system.
from requests.exceptions import ConnectionError
import json

# 200 = OK, 401 = Not Authorized
OK=200
AUTH=401

hdr = {} # authorization token and other header values go in here.

apiroot = "/"

doforce = 1

host = "localhost:54993"
if len(sys.argv)>1:
    host = sys.argv[1]
if len(sys.argv)>2:
    apiroot = sys.argv[2]
    doforce = 0



# URL Definitions:

auth1 = "http://" + host + apiroot +"authorize/login"
auth2 = "http://" + host + apiroot +"authorize/login?username=TEST&password=SECRET"
authtest1 = "http://" + host + apiroot +"authorizetest/test"


# functions area

def xfmt(url):
    apass=url.find("pass")
    if (apass>0):
        return url[0:apass+3]+"..."
    else:
        return url
    
def get_expect(url,xheaders=None,status_code=OK):
    try:
        response = requests.get(url,headers= xheaders)
        if response.status_code!=status_code:
            print("expected status "+str(status_code)+" but got "+str(response.status_code)+" for GET "+xfmt(url))
            sys.exit(1)
        print("GET OK: "+str(response.status_code)+" "+xfmt(url))
        return response.text
    except ConnectionError as e:
        print("ERROR: Unable to communicate with "+host)
        print("details: "+str(e))
        sys.exit(1)
        

def post_expect(url,data=None,header={},status_code=200):
    try:
        response = requests.post(url,data,header)
        if response.status_code!=status_code:
            print("expected status "+str(status_code)+" but got "+str(response.status_code)+" for POST "+xfmt(url))
            sys.exit(1)
        print("POST OK: "+str(response.status_code)+" "+xfmt(url))
        return response.text
    except ConnectionError as e:
        print("ERROR: Unable to communicate with "+host)
        print("details: "+str(e))

        sys.exit(1)




#get_expect(status1, None, AUTH) # 401

# ensure that authentication mechanism denies unqualified Get request.
#get_expect(patient1,None,401)
def auth():
    global hdr
    #get_expect(auth1)  # TODO: After updating.
    authurl = auth2 # auth2 or authx2
    get_expect(authurl, None, OK) # was auth2

    # POST /api/v3/login... : Get bearer token, returns { 'success': True } if it works.
    res = json.loads( post_expect(authurl) )
    success = res["success"]
    if not success:
        print("authorize fail: success flag missing, or authorization check failed")
        print(repr(res))
        sys.exit(1)
    token = res["access_token"]
    if (len(token)<60):
        print("authorize fail: token missing too short.")
        sys.exit(1)
    else:
        print("authorization token received... "+token[0:10]+"..." )
    hdr = { 'Authorization': 'Bearer ' + token }
    res2 = json.loads(get_expect(authtest1,hdr))
    print("authtest:"+repr(res2))



# MAIN SCRIPT AREA starts

auth()          # get hdr = { ... auth token info }

   

# If nothing terminated, or raised, Success.
print( "Success." )
sys.exit(0)
    
