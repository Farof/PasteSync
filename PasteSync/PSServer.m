//
//  PSServer.m
//  PasteSync
//
//  Created by Mathieu Merdy on 13/04/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import "PSServer.h"

@implementation PSServer

@synthesize sock = _sock;
@synthesize sin = _sin;
@synthesize port = _port;
@synthesize ref = _ref;
//@synthesize callback = _callback;

-(PSServer *) init {
    NSLog(@"init server");

    // tcp socket
    // https://developer.apple.com/library/ios/documentation/NetworkingInternet/Conceptual/NetworkingTopics/Articles/UsingSocketsandSocketStreams.html#//apple_ref/doc/uid/%28null%29-SW8

    int err = 0;
    
    // declare socket
    self.sock = socket(PF_INET, SOCK_STREAM, IPPROTO_TCP);

    // bind socket to port
    struct sockaddr_in sin;
    memset(&sin, 0, sizeof(sin));
    sin.sin_len = sizeof(sin);
    sin.sin_family = AF_INET;
    sin.sin_port = htons(0);
    sin.sin_addr.s_addr = INADDR_ANY;
    err = bind(self.sock, (struct sockaddr *)&sin, sizeof(sin));
    if (err < 0) {
        NSLog(@"Error binding socket: %i", err);
        return self;
    }
    self.sin = sin;
    
    // get port number
    socklen_t len = sizeof(sin);
    err = getsockname(self.sock, (struct sockaddr *)&sin, &len);
    if (err < 0) {
        NSLog(@"Error getting socket port number: %i", err);
        return self;
    }
    self.port = ntohs(sin.sin_port);
    NSLog(@"socket created on port %u", self.port);

    return self;
}

-(PSServer *) initAndStart {
    self = [self init];

    // register service
    [self registerService];
    // listen to socket
    listen(self.sock, 8);

    return self;
}

// bonjour service
// https://developer.apple.com/library/ios/documentation/NetworkingInternetWeb/Conceptual/NetworkingOverview/Discovering,Browsing,AndAdvertisingNetworkServices/Discovering,Browsing,AndAdvertisingNetworkServices.html#//apple_ref/doc/uid/TP40010220-CH9-SW1
-(void) registerService {
    int err = 0;
    
    // dns-sd -B _pastesync._tcp
    // create service
    char* key = "name";
    char* value = "PasteSync";
    uint8_t len = (uint8_t)strlen(value);

    TXTRecordRef txtref;
    TXTRecordCreate(&txtref, 0, NULL);
    TXTRecordSetValue(&txtref, key, len, value);

    err = DNSServiceRegister(&(self->_ref), 0, 0, NULL, "_pastesync._tcp", NULL, NULL, self.port,
                             TXTRecordGetLength(&txtref), TXTRecordGetBytesPtr(&txtref), serviceCallback, NULL);
    if (err < 0) {
        NSLog(@"Error registering service: %i", err);
        return;
    }
    TXTRecordDeallocate(&txtref);
}

-(void) unregisterService {
    if (self.ref) {
        DNSServiceRefDeallocate(self.ref);
    }
}

void serviceCallback(DNSServiceRef ref, DNSServiceFlags flags, DNSServiceErrorType errorCode,
                     const char * name, const char * regtype, const char * domain, void * context) {
    NSLog(@"plop yeah");
}

@end
