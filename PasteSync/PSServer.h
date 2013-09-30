//
//  PSServer.h
//  PasteSync
//
//  Created by Mathieu Merdy on 13/04/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import <Foundation/Foundation.h>
#import <netinet/in.h>
#import <sys/socket.h>
#import <dns_sd.h>

@interface PSServer : NSObject

@property (nonatomic) int sock;
@property (nonatomic) struct sockaddr_in sin;
@property (nonatomic) uint16_t port;
@property (nonatomic) DNSServiceRef ref;
//@property (nonatomic) DNSServiceRegisterReply callback;

-(PSServer *)initAndStart;
-(void)registerService;
-(void)unregisterService;

void serviceCallback(DNSServiceRef, DNSServiceFlags, DNSServiceErrorType,
                     const char *, const char *, const char *, void *);

@end
