/*
 * 
 * All the resources for this project: https://www.hackster.io/Aritro
 * Modified by Aritro Mukherjee
 * 
 * 
 */
 
#include <SPI.h>
#include <MFRC522.h>
 
#define SS_PIN 10
#define RST_PIN 9
MFRC522 mfrc522(SS_PIN, RST_PIN);   // Create MFRC522 instance.

bool button1;
bool button2;
 
void setup() 
{
  digitalWrite(A0, LOW);
  
  Serial.begin(9600);   // Initiate a serial communication
  Serial.setTimeout(8);
  SPI.begin();      // Initiate  SPI bus
  mfrc522.PCD_Init();   // Initiate MFRC522
  pinMode(2, OUTPUT);
  pinMode(6, OUTPUT);
  digitalWrite(2, LOW);
  pinMode(A0, OUTPUT);
  pinMode(A1, OUTPUT);
}
void loop() 
{
  String output = Serial.readString();
  
  while(Serial.available() == 0)
  {
    int a5 = analogRead(A5);
    int a4 = analogRead(A4);
    
    if(a5 > 500 && button1 == false)
    {
      Serial.println("prev\n");
      button1 = true;
    }
    if(a5 < 500)
    {
      button1 = false;
    }

    if(a4 > 500 && button2 == false)
    {
      Serial.println("next\n");
      button2 = true;
    }
    if(a4 < 500)
    {
      button2 = false;
    }
    ReadCard();

    //Serial.println((String)analogRead(A1) + " " + (String)analogRead(A0));

    delay(10);
  }
} 

void ReadCard()
{
  // Look for new cards
  if ( ! mfrc522.PICC_IsNewCardPresent()) 
  {
    return;
  }
  // Select one of the cards
  if ( ! mfrc522.PICC_ReadCardSerial()) 
  {
    return;
  }
  //Show UID on serial monitor
  String content= "";
  byte letter;
  for (byte i = 0; i < mfrc522.uid.size; i++) 
  {
     //Serial.print(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " ");
     //Serial.print(mfrc522.uid.uidByte[i], HEX);
     content.concat(String(mfrc522.uid.uidByte[i] < 0x10 ? " 0" : " "));
     content.concat(String(mfrc522.uid.uidByte[i], HEX));
  }
  content.toUpperCase();
  if(content != "")
  {
    Serial.println(content + "\n");
    delay(500);
  }
  if(content == " 84 B3 64 4F")
  {
    digitalWrite(A1, HIGH);
  }
  else
  {
    digitalWrite(A1, LOW);
  }
}
