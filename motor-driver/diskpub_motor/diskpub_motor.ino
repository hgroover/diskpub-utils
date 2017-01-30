// Motor control for disk publisher
// HP print engine motor driver died. Apparently they use
// cheap DC bipolar motors with feedback, in this case a clear disk 
// with markings passing between an IR LED / IR sensor pair.

int LED=13;
volatile int state = LOW;
char getstr;
int in1=8;
int in2=7;
int in3=6;
int in4=5;
int ENA=10;
int ENB=5;
//int ABS=135;
unsigned long onTime = 0;
volatile int timedOn = 0;
unsigned long offTime = 0;

void _mForward()
{
//analogWrite (ENA, 200); //DOLOČIŠ HTROST MOTORJA
digitalWrite(ENA,HIGH);
//digitalWrite(ENB,HIGH);
///digitalWrite(in1,LOW);
///digitalWrite(in2,HIGH);
//digitalWrite(in3,LOW);
//digitalWrite(in4,HIGH);
Serial.println("go forward!");
digitalWrite(in1, HIGH);
digitalWrite(in2, LOW);
// set speed to 200 out of possible range 0~255

}
void _mBack()
{
//analogWrite (ENA, 200); //DOLOČIŠ HTROST MOTORJA
digitalWrite(ENA,HIGH);
//digitalWrite(ENB,HIGH);
///digitalWrite(in1,HIGH);
///digitalWrite(in2,LOW);
//digitalWrite(in3,HIGH);
//digitalWrite(in4,LOW);
Serial.println("go back!");
digitalWrite(in1, LOW);
digitalWrite(in2, HIGH);
}
void _mleft()
{
//analogWrite (ENB, 200); //DOLOČIŠ HTROST MOTORJA
digitalWrite(ENA,HIGH);
digitalWrite(ENB,HIGH);
//digitalWrite(in1,LOW);
//digitalWrite(in2,HIGH);
digitalWrite(in3,HIGH);
digitalWrite(in4,LOW);
Serial.println("go left!");
}
void _mright()
{
//analogWrite (ENA, 200); //DOLOČIŠ HTROST MOTORJA
digitalWrite(ENA,HIGH);
digitalWrite(ENB,HIGH);
//digitalWrite(in1,HIGH);
//digitalWrite(in2,LOW);
digitalWrite(in3,LOW);
digitalWrite(in4,HIGH);
Serial.println("go right!");
}
void _mStop()
{
digitalWrite(ENA,LOW);
//digitalWrite(ENB,LOW);
digitalWrite(in1,LOW); //jaz dodal - drugače tevrstice ni
digitalWrite(in2,LOW); //jaz dodal - drugače tevrstice ni
//digitalWrite(in3,LOW); //jaz dodal - drugače tevrstice ni
//digitalWrite(in4,LOW); //jaz dodal - drugače tevrstice ni
Serial.println("Stop!");
}
void stateChange()
{
state = !state;
digitalWrite(LED, state);
}
void setup()
{
pinMode(LED, OUTPUT);
Serial.begin(9600);
pinMode(in1,OUTPUT);
pinMode(in2,OUTPUT);
//pinMode(in3,OUTPUT);
//pinMode(in4,OUTPUT);
pinMode(ENA,OUTPUT);
//pinMode(ENB,OUTPUT);
_mStop();
}


void loop()
{
getstr=Serial.read();
if(getstr=='F')
{
_mForward();
delay(10);
}
else if (getstr=='f' && timedOn == 0)
{
  onTime = millis();
  timedOn = 1;
  offTime = onTime + 500;
  _mForward();  
}
else if(getstr=='B')
{
_mBack();
delay(10);
}
else if (getstr =='b' && timedOn == 0)
{
  onTime = millis();
  timedOn = -1;
  offTime = onTime + 500;
  _mBack();
}
else if(getstr=='L')
{
_mleft();
delay(10);
}
else if(getstr=='R')
{
_mright();
delay(10);
}
else if(getstr=='A')
{
stateChange();
}
else if(getstr=='S')
{
_mStop();
}
else if (timedOn)
{
  if (millis() >= offTime)
  {
    timedOn = 0;
    _mStop();
  }
}
}

