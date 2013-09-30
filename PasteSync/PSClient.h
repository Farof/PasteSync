//
//  PSClient.h
//  PasteSync
//
//  Created by Mathieu Merdy on 29/09/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import <Foundation/Foundation.h>

@interface PSClient : NSObject <NSNetServiceBrowserDelegate, NSNetServiceDelegate>

@property (nonatomic, strong) NSNetServiceBrowser *netBrowser;
@property (nonatomic, strong) NSString *serviceType;
@property (nonatomic, strong) NSString *domain;
@property (nonatomic, strong) NSMutableDictionary *services;

-(NSString *)fqdnForService:(NSNetService *)aNetService;
-(void)startBrowsing;
-(void)stopBrowsing;

@end
