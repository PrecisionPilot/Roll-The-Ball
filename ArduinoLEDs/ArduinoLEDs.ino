//18 >= count > 0
//count >= ledsOn >= 0
const int count = 15;
int ledsOn;

//Offset value from the beginning (pin 2) or end (pin 19/A5)
const int offset = 2;
bool reverse = false;

bool debugMode = false;

void setup() {
  Serial.begin(1000000);
  Serial.setTimeout(8);

  //Set all the pins to output
  for(int i = 2; i <= 19; i++)
  {
    pinMode(i, OUTPUT);
  }
}
void loop() {
  String output = Serial.readString();
  
  int value1 = output.substring(0, 3).toInt();
  int value2 = output.substring(3, 6).toInt();
  if (!debugMode) { ledsOn = map(value1, 0, 255, 0, count); }
  else { ledsOn = value1; } 
  
  //clear
  for (int i = offset + 2; i < count + (offset + 2); i++)
  {
    digitalWrite(i, LOW);
  }
  
  //Normal
  if (!reverse){
    for (int i = offset + 2; i < ledsOn + (offset + 2); i++)
    {
      digitalWrite(i, HIGH);
    }
  }
  //Reverse
  else {
    for (int i = 19 - offset; i > 19 - ledsOn - offset; i--)
    {
      digitalWrite(i, HIGH);
    }
  }
  
  if (value2 > 0)
    analogWrite(3, value2);
  Serial.flush();
  while(Serial.available() == 0){}
}
