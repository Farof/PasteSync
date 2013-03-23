//
//  AppDelegate.m
//  PasteSync
//
//  Created by Mathieu Merdy on 22/03/13.
//  Copyright (c) 2013 Mathieu Merdy. All rights reserved.
//

#import "AppDelegate.h"

@implementation AppDelegate

@synthesize statusBar = _statusBar;
@synthesize statusMenu = _statusMenu;
@synthesize pasteLabel = _pasteLabel;
@dynamic latestPaste;

-(NSString *) latestPaste {
    NSPasteboard *pasteboard = [NSPasteboard generalPasteboard];
    NSArray *classes = [[NSArray alloc] initWithObjects:[NSString class], nil];
    NSDictionary *options = [NSDictionary dictionary];
    NSArray *copiedItems = [pasteboard readObjectsForClasses:classes options:options];

    if (copiedItems != nil && copiedItems.count > 0) {
        return [copiedItems objectAtIndex:0];
    }

    return nil;
}

- (void)applicationDidFinishLaunching:(NSNotification *)aNotification
{
    // Insert code here to initialize your application
}

-(void) awakeFromNib {
    self.statusBar = [[NSStatusBar systemStatusBar] statusItemWithLength:NSVariableStatusItemLength];
    self.statusBar.title = @"PS";
    // self.statusBar.image =
    self.statusBar.menu = self.statusMenu;
    self.statusBar.highlightMode = YES;

    NSString *latest = self.latestPaste;
    if (latest != nil) {
        if (latest.length > 40) {
            NSLog(@"trim: %@", latest);
            latest = [latest substringToIndex:40];
            NSArray *compo = [NSArray arrayWithObjects:latest, @"…", nil];
            latest = [compo componentsJoinedByString:@""];
        }
        self.pasteLabel.title = latest;
    }
}

- (IBAction)print:(id)sender {
    NSLog(@"print");

    NSString *latestPaste = self.latestPaste;

    if (latestPaste != nil) {
        NSLog(@"Latest paste: %@", latestPaste);
    } else {
        NSLog(@"Nothing in pasteboard");
    }
}
@end
