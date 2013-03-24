//
//  PSMonitor.h
//  PasteSync
//
//  Created by Mathieu Merdy on 24/03/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@interface PSMonitor : NSObject

@property (strong, nonatomic) NSTimer *timer;
@property (strong, nonatomic, readonly) NSString *currentData;
@property (strong, nonatomic) NSString *latestData;

-(void)startMonitoring;
-(void)stopMonitoring;
-(void)queryPasteboard;

@end
