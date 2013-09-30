//
//  PSClient.m
//  PasteSync
//
//  Created by Mathieu Merdy on 29/09/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import "PSClient.h"

@implementation PSClient

@synthesize netBrowser = _netBrowser;
@synthesize serviceType = _serviceType;
@synthesize domain = _domain;
@synthesize services = _services;

-(PSClient *) init {
    self.services = [[NSMutableDictionary alloc] init];
    self.serviceType = @"_pastesync._tcp.";
    self.domain = @"local.";
    
    self.netBrowser = [[NSNetServiceBrowser alloc] init];
    [self.netBrowser setDelegate:self];
    
    [self startBrowsing];

    return self;
}

-(void) startBrowsing {
  [self.netBrowser searchForServicesOfType:self.serviceType inDomain:self.domain];
}

-(void) stopBrowsing {
    [self.netBrowser stop];
}

-(NSString *) fqdnForService:(NSNetService *)aNetService {
    return [[NSString alloc] initWithFormat:@"%@.%@%@", aNetService.name, aNetService.type, aNetService.domain];
}

-(void) netServiceBrowserWillSearch:(NSNetServiceBrowser *)aNetServiceBrowser {
    PSClient *client = (PSClient *) aNetServiceBrowser.delegate;
    NSLog(@"searching for net service %@%@", client.serviceType, client.domain);
}

-(void) netServiceBrowser:(NSNetServiceBrowser *)aNetServiceBrowser didFindService:(NSNetService *)aNetService moreComing:(BOOL)moreComing {
    if (moreComing) {
        NSLog(@"more coming");
    } else {
        NSLog(@"all services found");
        [aNetServiceBrowser stop];
    }
    
    NSString *fqdn = [self fqdnForService:aNetService];
    NSLog(@"service found: %@", fqdn);
    [self.services setObject:aNetService forKey:fqdn];

    [aNetService setDelegate:self];
    [aNetService resolveWithTimeout:5.0];

    NSString *name = [[NSString alloc] initWithData:[aNetService TXTRecordData] encoding:NSUTF8StringEncoding];
    NSLog(@"txt record: %@", name);
    NSDictionary *dict = [NSNetService dictionaryFromTXTRecordData:[aNetService TXTRecordData]];
    NSLog(@"record dictionnary count: %ld", dict.count);
    
    NSLog(@"connected to %ld devices", self.services.count);
}

-(void) netServiceBrowser:(NSNetServiceBrowser *)aNetServiceBrowser didRemoveService:(NSNetService *)aNetService moreComing:(BOOL)moreComing {
    NSString *fqdn = [self fqdnForService:aNetService];
    NSLog(@"service removed: %@", fqdn);
    [self.services removeObjectForKey:fqdn];
        
    NSLog(@"connected to %ld devices", self.services.count);
}

-(void) netServiceWillPublish:(NSNetService *)sender {
    NSLog(@"wil publish %@", [self fqdnForService:sender]);
}

-(void) netServiceWillResolve:(NSNetService *)sender {
    NSLog(@"wil resolve %@", [self fqdnForService:sender]);
}

-(void) netServiceDidPublish:(NSNetService *)sender {
    NSLog(@"did publish %@", [self fqdnForService:sender]);
}

-(void) netServiceDidResolveAddress:(NSNetService *)sender {
    NSLog(@"did resolve %@", [self fqdnForService:sender]);
    NSString *name = [[NSString alloc] initWithData:[sender TXTRecordData] encoding:NSUTF8StringEncoding];
    NSLog(@"txt record: %@", name);
    NSDictionary *dict = [NSNetService dictionaryFromTXTRecordData:[sender TXTRecordData]];
    NSLog(@"service app name: %@", [[NSString alloc] initWithData:[dict objectForKey:@"name"] encoding:NSUTF8StringEncoding]);
}

-(void) netService:(NSNetService *)sender didNotPublish:(NSDictionary *)errorDict {
    NSLog(@"did not publish %@", [self fqdnForService:sender]);
}

-(void) netService:(NSNetService *)sender didNotResolve:(NSDictionary *)errorDict {
    NSLog(@"did not resolve %@", [self fqdnForService:sender]);
}

-(void) netServiceDidStop:(NSNetService *)sender {
    NSLog(@"did stop %@", [self fqdnForService:sender]);
}

-(void) netService:(NSNetService *)sender didUpdateTXTRecordData:(NSData *)data {
    NSString *fqdn = [self fqdnForService:sender];
    NSString *txtrecord = [[NSString alloc] initWithData:data encoding:NSUTF8StringEncoding];
    NSLog(@"service %@ new txt record: %@", fqdn, txtrecord);
}

@end
