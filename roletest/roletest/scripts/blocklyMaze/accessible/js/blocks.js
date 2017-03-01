/**
 * @license
 * Visual Blocks Editor
 *
 * Copyright 2016 Google Inc.
 * https://github.com/google/blockly-games
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *   http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

/**
 * @fileoverview Blocks for the Blockly Games music demo.
 * @author sll@google.com (Sean Lip)
 */
'use strict';

goog.provide('Music.Blocks');

goog.require('Blockly');
goog.require('Blockly.JavaScript');


/**
 * Common HSV hue for all blocks in this category.
 */
Music.Blocks.HUE = 20;

Music.Blocks.NOTE_OPTIONS = [
  ["C3", "36"],
  ["D3", "38"],
  ["E3", "40"],
  ["F3", "41"],
  ["G3", "43"],
  ["A3", "45"],
  ["B3", "47"],
  ["C4", "48"],
  ["D4", "50"],
  ["E4", "52"],
  ["F4", "53"],
  ["G4", "55"],
  ["A4", "57"],
  ["B4", "59"]
];

var MUSIC_DUMMY_TOOLTIP = 'Dummy tooltip';
var MUSIC_DUMMY_HELPURL = 'Dummy help URL';

// Extensions to Blockly's language and JavaScript generator.

Blockly.Blocks['music_play_random_note'] = {
  /**
   * Block for playing a random music note.
   * @this Blockly.Block
   */
  init: function() {
    this.jsonInit({
      "message0": "play random note",
      "previousStatement": null,
      "nextStatement": null,
      "colour": Music.Blocks.HUE,
      "tooltip": MUSIC_DUMMY_TOOLTIP,
      "helpUrl": MUSIC_DUMMY_HELPURL
    });
  }
};

Blockly.JavaScript['music_play_random_note'] = function(block) {
  var LOWEST_PITCH = 36;
  var HIGHEST_PITCH = 60;

  var randomPitch =
      Math.floor(Math.random() * (HIGHEST_PITCH - LOWEST_PITCH) +
      LOWEST_PITCH);
  return 'addChord([' + randomPitch + '], 1);\n';
};

Blockly.Blocks['music_play_note'] = {
  /**
   * Block for playing a music note.
   * @this Blockly.Block
   */
  init: function() {
    this.jsonInit({
      "message0": "play note %1",
      "args0": [
        {
          "type": "field_dropdown",
          "name": "PITCH",
          "options": Music.Blocks.NOTE_OPTIONS
        }
      ],
      "previousStatement": null,
      "nextStatement": null,
      "colour": Music.Blocks.HUE,
      "tooltip": MUSIC_DUMMY_TOOLTIP,
      "helpUrl": MUSIC_DUMMY_HELPURL
    });
  }
};

Blockly.JavaScript['music_play_note'] = function(block) {
  // Play a single note.
  return 'addChord([' + block.getFieldValue('PITCH') + '], 1);\n';
};

Blockly.Blocks['music_play_note_with_duration'] = {
  /**
   * Block for playing a note with a specified duration.
   * @this Blockly.Block
   */
  init: function() {
    this.jsonInit({
      "message0": "play note %1 for %2 beat(s)",
      "args0": [
        {
          "type": "field_dropdown",
          "name": "PITCH",
          "options": Music.Blocks.NOTE_OPTIONS
        },
        {
          "type": "field_dropdown",
          "name": "DURATION",
          "options": [
            ["1", "1"],
            ["2", "2"],
            ["3", "3"],
            ["4", "4"],
            ["1/2", "0.5"],
            ["1/4", "0.25"],
            ["3/4", "0.75"]
          ]
        }
      ],
      "previousStatement": null,
      "nextStatement": null,
      "colour": Music.Blocks.HUE,
      "tooltip": MUSIC_DUMMY_TOOLTIP,
      "helpUrl": MUSIC_DUMMY_HELPURL
    });
  }
};

Blockly.JavaScript['music_play_note_with_duration'] = function(block) {
  // Play a single note, with the given duration.
  var code =
      'addChord([' + block.getFieldValue('PITCH') + '], ' +
      block.getFieldValue('DURATION') + ');\n';
  return code;
};
