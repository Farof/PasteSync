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
@synthesize psMonitor = _psMonitor;

-(void)applicationDidFinishLaunching:(NSNotification *)aNotification
{
    // Insert code here to initialize your application
}

-(void) awakeFromNib {
    self.psMonitor = [[PSMonitor alloc] init];

    self.statusBar = [[NSStatusBar systemStatusBar] statusItemWithLength:NSVariableStatusItemLength];
    self.statusBar.title = @"PS";
    // self.statusBar.image =
    self.statusBar.menu = self.statusMenu;
    self.statusBar.highlightMode = YES;

    [self updateLabel:self.psMonitor.currentData];
    [[NSNotificationCenter defaultCenter] addObserver:self selector:@selector(handleDataChange:) name:@"dataChanged" object:nil];
}

-(NSString *)normalizeString:(NSString *)str {
    if (str.length > 40) {
        str = [str substringToIndex:40];
        NSArray *compo = [NSArray arrayWithObjects:str, @"…", nil];
        str = [compo componentsJoinedByString:@""];
    }
    return str;
}

-(void)updateLabel:(NSString *)str {
    if (str != nil) {
        str = [self normalizeString:str];
        self.pasteLabel.title = str;
    }
}

-(void)handleDataChange:(NSNotification *)notification {
    NSString *str = (NSString*)notification.object;
    [self updateLabel:[self normalizeString:str]];
}

-(IBAction)print:(id)sender {
    NSLog(@"print");

    NSString *latestPaste = self.psMonitor.currentData;

    if (latestPaste != nil) {
        NSLog(@"Latest paste: %@", latestPaste);
    } else {
        NSLog(@"Nothing in pasteboard");
    }
}

@end
