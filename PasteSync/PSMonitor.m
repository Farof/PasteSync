//
//  PSMonitor.m
//  PasteSync
//
//  Created by Mathieu Merdy on 24/03/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import "PSMonitor.h"
#import <ApplicationServices/ApplicationServices.h>

@implementation PSMonitor

@synthesize timer = _timer;
@synthesize latestData = _latestData;
@dynamic currentData;

-(PSMonitor *) init {
    NSLog(@"init monitor");

    self.latestData = self.currentData;

    return self;
}

-(PSMonitor *) initAndStart {
    self = [self init];
    [self startMonitoring];
    return self;
}

-(NSString *)currentData {
    NSPasteboard *pasteboard = [NSPasteboard generalPasteboard];
    NSArray *classes = [[NSArray alloc] initWithObjects:[NSString class], nil];
    NSDictionary *options = [NSDictionary dictionary];
    NSArray *copiedItems = [pasteboard readObjectsForClasses:classes options:options];

    if (copiedItems != nil && copiedItems.count > 0) {
        return [copiedItems objectAtIndex:0];
    }

    return nil;
}

-(void)startMonitoring {
    self.timer = [NSTimer scheduledTimerWithTimeInterval:3 target:self selector:@selector(queryPasteboard) userInfo:nil repeats:YES];
}

-(void)stopMonitoring {
    if (self.timer.isValid == YES) {
        [self.timer invalidate];
        self.timer = nil;
    }
}

-(void)queryPasteboard {
    NSString *newData = self.currentData;
    if (![newData isEqualToString:self.latestData]) {
        NSLog(@"Pasteboard content changed: %@", newData);
        self.latestData = newData;
        [[NSNotificationCenter defaultCenter] postNotificationName:@"dataChanged" object:newData userInfo:nil];
    }
}

@end
