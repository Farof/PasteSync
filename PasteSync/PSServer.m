//
//  PSServer.m
//  PasteSync
//
//  Created by Mathieu Merdy on 13/04/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import "PSServer.h"

@implementation PSServer

-(PSServer *) init {
    NSLog(@"init server");
}

-(PSServer *) initAndStart {
    self = [self init];

    // start

    return self;
}

@end
