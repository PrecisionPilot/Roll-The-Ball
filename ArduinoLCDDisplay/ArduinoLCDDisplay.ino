#include <LiquidCrystal.h>

unsigned long Time;

int row;

String lcdTop;
String lcdBottom;

const int red = 9;
const int green = 11;
const int greenBrightness = 20;
const int blue = 12;
const int ground = 10;

// initialize the library by associating any needed LCD interface pin
// with the arduino pin number it is connected to
const int rs = 2, en = 4, d4 = 5, d5 = 6, d6 = 7, d7 = 8;
const int vo = 3;

LiquidCrystal lcd(rs, en, d4, d5, d6, d7);

void setup() {
  pinMode(red, OUTPUT);
  pinMode(green, OUTPUT);
  pinMode(blue, OUTPUT);
  pinMode(ground, OUTPUT);
  digitalWrite(ground, LOW);
  pinMode(vo, OUTPUT);
  analogWrite(vo, 100);
  
  // set up the LCD's number of columns and rows:
  lcd.begin(16, 2);
  lcd.setCursor(0, 0);
  Serial.begin(9600);
  Serial.setTimeout(8);
}

void loop() {
  long prevTime = Time;
  while(Serial.available() == 0)//serial dormant
  {
    
    
    Time = millis();
    if(Time >= 100 + prevTime)//wait 50ms
    {
      digitalWrite(blue, LOW);
    }
  }
  digitalWrite(blue, HIGH);
  
  String output = Serial.readString();
  int outputLength = output.length();
  String headOutput = output.substring(0, 2);
  
  
  if(headOutput == "/1")
  {
    lcd.setCursor(0, 0);
    lcd.print("   Connected!   ");
    analogWrite(green, greenBrightness);
  }
  else if(headOutput == "/0")
  {
    lcd.clear();
    lcdTop = "";
    lcdBottom = "";
    digitalWrite(green, LOW);
  }
  else
  {
    if(headOutput == "/c")
    {
      lcd.clear();
  
      lcdTop = "";
      lcdBottom = "";
  
      if(outputLength > 2)
      {
        lcd.print(output.substring(2, outputLength));
      }
    }
    else if(headOutput == "/n")
    {
      lcdBottom = output.substring(2, outputLength);
      lcd.clear();
      lcd.setCursor(0, 1);
      lcd.print(lcdBottom);
      lcd.setCursor(0, 0);
      lcd.print(lcdTop);
    }
    else
    {
      lcdTop = output;
      lcd.clear();
      lcd.setCursor(0, 0);
      lcd.print(lcdTop);
      lcd.setCursor(0, 1);
      lcd.print(lcdBottom);
    }
  }
  Serial.flush();
}
