//
//  AppDelegate.h
//  PasteSync
//
//  Created by Mathieu Merdy on 22/03/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import <Cocoa/Cocoa.h>

@interface AppDelegate : NSObject <NSApplicationDelegate>

@property (weak) IBOutlet NSMenu *statusMenu;

@property (strong, nonatomic) IBOutlet NSStatusItem *statusBar;

@property (strong, nonatomic) IBOutlet NSMenuItem *pasteLabel;

@property (strong, nonatomic, readonly) NSString *latestPaste;

- (IBAction)print:(id)sender;

@end
