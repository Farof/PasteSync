//
//  AppDelegate.h
//  PasteSync
//
//  Created by Mathieu Merdy on 22/03/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import <Cocoa/Cocoa.h>
#import "PSMonitor.h"

@interface AppDelegate : NSObject <NSApplicationDelegate>

@property (weak) IBOutlet NSMenu *statusMenu;

@property (strong, nonatomic) IBOutlet NSStatusItem *statusBar;

@property (strong, nonatomic) IBOutlet NSMenuItem *pasteLabel;

@property (strong, nonatomic) PSMonitor *psMonitor;

-(IBAction)print:(id)sender;
-(NSString *)normalizeString:(NSString *)str;
-(void)updateLabel:(NSString *)str;
-(void)handleDataChange:(NSNotification *)notification;

@end
