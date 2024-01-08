unit MainUnit;

interface

uses
  Windows, Messages, SysUtils, Variants, Classes, Graphics, Controls, Forms,
  Dialogs, IniFiles, StdCtrls, FileCtrl, Grids, ComCtrls;

type
  {»„ÓÍ}
  TPlayer = record
    Name: string[22];
    Age: byte;
    Country: byte;
    YouthRating: byte;
    Rating: byte;
    Param: array [1..7] of integer;{P,H,V,C,T,S,F}
    Amplua: byte;
    Rasa: byte;
    Number: byte;
    Flag: boolean;
  end;
  {“ÂÌÂ}
  TTrener = record
    Name: string[24];
    Age: byte;
    Country: byte;
    Rating: byte;
    Tactic: byte;
    Flag: boolean;
  end;
  { ÓÏ‡Ì‰‡}
  TTeam = record
    Name: string[18];
    Country: byte;
    Forma: array [1..2,1..5] of byte;
    Trener: TTrener;
    Player: array [1..30] of TPlayer;
    TeamClass: integer;
  end;
  TForm1 = class(TForm)
    ComboBox1: TComboBox;
    ComboBox2: TComboBox;
    ComboBox7: TComboBox;
    ComboBox8: TComboBox;
    ComboBox10: TComboBox;
    ComboBox3: TComboBox;
    ComboBox4: TComboBox;
    ComboBox5: TComboBox;
    ComboBox6: TComboBox;
    OpenDialog1: TOpenDialog;
    ComboBox11: TComboBox;
    ComboBox12: TComboBox;
    Button3: TButton;
    ProgressBar1: TProgressBar;
    lblSwosPath: TLabel;
    lblSwosPathCaption: TLabel;
    lstSwosPath: TDirectoryListBox;
    btnCareer: TButton;
    btnRecordTeamToCareer: TButton;
    Label1: TLabel;
    Label2: TLabel;
    procedure FormCreate(Sender: TObject);
    procedure RecordCustoms;
    procedure ChangeTeams;
    procedure CreateTactics;
    procedure Transfers;
    procedure ChangeAge;
    procedure DeletePlayer(Num: integer);
    procedure DeleteTrener(Num: integer);
    procedure ReadPlayers;
    procedure ReadTreners;
    procedure ReadTeams;
    procedure WritePlayers;
    procedure WriteTreners;
    procedure WriteTeams;
    procedure ReadOriginalTeams;
    procedure ComboBox11Change(Sender: TObject);
    procedure Button3Click(Sender: TObject);
    procedure lstSwosPathChange(Sender: TObject);
    procedure btnCareerClick(Sender: TObject);
    procedure FormClose(Sender: TObject; var Action: TCloseAction);
    procedure btnRecordTeamToCareerClick(Sender: TObject);
  private
    { Private declarations }
  public
    { Public declarations }
  end;

var PROGDIR: AnsiString;
    NumTeams: array of integer;
    NumPlayers: integer;
    NumTreners: integer;
    Teams: array of array of TTeam;
    TeamTransfers: array of array of array [0..7] of array of array [1..2] of integer;
    NumTeamTransfers: array of array of array [0..7] of integer;
    Players: array of TPlayer;
    Treners: array of TTrener;
    FSWS: file of byte;
    FPlayer: file of TPlayer;
    FTrener: file of TTrener;
    FTeam: file of TTeam;
    FCountry: file of integer;
    c: byte;
    FIni: TIniFile;
    Year: integer;
    Form1: TForm1;

    ProvAmp: array [0..7] of integer;

implementation

{$R *.dfm}

{‘”Õ ÷»ﬂ "Œœ–≈ƒ≈À≈Õ»≈ —À”◊¿…ÕŒ√Œ ◊»—À¿ ¬ «¿ƒ¿ÕÕŒÃ ƒ»¿œ¿«ŒÕ≈"}
function Rand(a,b: integer): integer;
begin
 Rand:=random(b-a+1)+a;
end;

{œ–Œ÷≈ƒ”–¿ Õ¿ —Œ¡€“»≈ "—Œ«ƒ¿Õ»≈ ‘Œ–Ã€"}
procedure TForm1.FormCreate(Sender: TObject);
var ini: TIniFile;
begin
 randomize;
 PROGDIR := ExtractFilePath(ParamStr(0));
 ini := TIniFile.Create(PROGDIR + '\Career.ini');
 try
  lstSwosPath.Directory := ini.ReadString('Main', 'SwosPath', '');
  Year := ini.ReadInteger('Main', 'Year', 0);
  if (Year > 0) then
  begin
   btnCareer.Caption := 'NEXT SEASON';
   Caption := 'SWOS Career Mod. Year: ' + IntToStr(Year);
  end;
 finally
  ini.Free;
 end;
 lblSwosPath.Caption := lstSwosPath.Directory;
end;

{œ–Œ÷≈ƒ”–¿ "«¿œ»—‹ ‘¿…À¿ CUSTOMS.EDT"}
procedure TForm1.RecordCustoms;
var i,j: integer;
    s: string;

procedure RecordTeam(Nm,Num: integer);
var i,j: integer;

procedure RecordTrener;
var i: integer;
begin
 for i:=1 to 24 do
  begin
   c:=ORD(Teams[Nm,Num].Trener.Name[i]);
   if i>length(Teams[Nm,Num].Trener.Name) then c:=0;
   Seek(FSWS,Num*684+i+37);
   Write(FSWS,c);
  end;
 c:=Teams[Nm,Num].Trener.Tactic;
 Seek(FSWS,Num*684+26);
 Write(FSWS,c);
 for i:=0 to 15 do
  begin
   c:=i;
   Seek(FSWS,Num*684+i+62);
   Write(FSWS,c);
  end;
end;

procedure RecordPlayer;
var i,j,k: integer;
begin
 for i:=1 to 30 do
  if Teams[Nm,Num].Player[i].Number>0 then
   begin
    k:=Teams[Nm,Num].Player[i].Number-1;
    for j:=1 to 22 do
     begin
      c:=ORD(Teams[Nm,Num].Player[i].Name[j]);
      if j>length(Teams[Nm,Num].Player[i].Name) then c:=0;
      Seek(FSWS,Num*684+k*38+j+80);
      Write(FSWS,c);
     end;
    c:=Teams[Nm,Num].Player[i].Country;
    Seek(FSWS,Num*684+k*38+78);
    Write(FSWS,c);
    c:=Teams[Nm,Num].Player[i].Rating;
    Seek(FSWS,Num*684+k*38+110);
    Write(FSWS,c);
    c:=Teams[Nm,Num].Player[i].Number;
    Seek(FSWS,Num*684+k*38+80);
    Write(FSWS,c);
    c:=Teams[Nm,Num].Player[i].Amplua*32 + Teams[Nm,Num].Player[i].Rasa*8;
    Seek(FSWS,Num*684+k*38+104);
    Write(FSWS,c);
    c:=8+Teams[Nm,Num].Player[i].Param[1];
    Seek(FSWS,Num*684+k*38+106);
    Write(FSWS,c);
    if Teams[Nm,Num].Player[i].Amplua=3 then c:=(8+Teams[Nm,Num].Player[i].Param[3])*16 + 8+Teams[Nm,Num].Player[i].Param[5]
		                     else c:=(8+Teams[Nm,Num].Player[i].Param[3])*16 + 8+Teams[Nm,Num].Player[i].Param[2];
    Seek(FSWS,Num*684+k*38+107);
    Write(FSWS,c);
    if Teams[Nm,Num].Player[i].Amplua=3 then c:=(8+Teams[Nm,Num].Player[i].Param[2])*16 + 8+Teams[Nm,Num].Player[i].Param[4]
                                     else c:=(8+Teams[Nm,Num].Player[i].Param[5])*16 + 8+Teams[Nm,Num].Player[i].Param[4];
    Seek(FSWS,Num*684+k*38+108);
    Write(FSWS,c);
    c:=(8+Teams[Nm,Num].Player[i].Param[6])*16 + 8+Teams[Nm,Num].Player[i].Param[7];
    Seek(FSWS,Num*684+k*38+109);
    Write(FSWS,c);
   end;
end;

begin
 for i:=1 to 18 do
  begin
   c:=ORD(Teams[Nm,Num].Name[i]);
   if i>length(Teams[Nm,Num].Name) then c:=0;
   Seek(FSWS,Num*684+i+6);
   Write(FSWS,c);
  end;
 c:=StrToInt(ComboBox1.Items.Strings[Teams[Nm,Num].Country]);
 Seek(FSWS,Num*684+2);
 Write(FSWS,c);
 for i:=1 to 2 do
  for j:=1 to 5 do
   begin
    c:=Teams[Nm,Num].Forma[i,j];
    Seek(FSWS,Num*684+i*5+j+22);
    Write(FSWS,c);
   end;
 RecordTrener;
 RecordPlayer;
end;

begin
 for i:=0 to ComboBox5.Items.Count-1 do
  begin
   s:=ComboBox5.Items.Strings[i];
   if length(s)=1 then s:='00'+s;
   if length(s)=2 then s:='0'+s;
   AssignFile(FSWS, lblSwosPath.Caption + '\data\team.' + s);
   {$I-}
   Reset(FSWS);
   {$I+}
   if IOResult=0 then
    begin
     for j:=0 to NumTeams[i]-1 do RecordTeam(i,j);
     CloseFile(FSWS);
    end;
  end;
end;

{‘”Õ ÷»ﬂ "Œœ–≈ƒ≈À≈Õ»≈ œŒ¬€ÿ≈Õ»ﬂ/œŒÕ»∆≈Õ»ﬂ ’¿–¿ “≈–»—“»  »√–Œ ¿"}
function ParamPlus(Player: TPlayer; Rating: integer): TPlayer;
const LConst: array [1..7,1..7] of integer =
      ((0,0,0,0,1,1,-1),
       (0,0,0,0,1,1,-1),
       (0,1,0,0,1,0,-1),
       (1,-1,0,1,0,1,-1),
       (1,-1,0,1,0,1,-1),
       (1,0,0,0,0,0,-1),
       (-1,1,0,0,-1,0,1));

var TMPPlayer: TPlayer;
    i: integer;
    Level: array [1..7] of integer;
    SumL: integer;
    TmpRating: integer;
    Temp,Tmp,Tempo: integer;
    flag: boolean;

function Stepen(O,P: integer): integer;
var TmpStepen: integer;
    i: integer;
begin
 TmpStepen:=1;
 for i:=1 to P do
  TmpStepen:=TmpStepen*O;
 Stepen:=TmpStepen;
end;

begin
 TMPPlayer := Player;
 TmpRating := Rating;
 if TMPPlayer.Amplua>0 then
  begin
   if TmpRating>0 then Temp:=1  else
   if TmpRating<0 then Temp:=-1 else Temp:=0;
   SumL:=0;
   for i:=1 to 7 do
    begin
     Level[i]:=Stepen(2,abs(LConst[TMPPlayer.Amplua,i]+Temp));
     SumL:=SumL+Level[i];
    end;
   while TmpRating<>0 do
    begin
     flag:=true;
     Tempo:=Rand(0,SumL-1);
     Tmp:=0;
     for i:=1 to 7 do
      begin
       Tmp:=Tmp+Level[i];
       if (Tempo>=SumL-Tmp) and (Tempo<>-1) then
        begin
         TMPPlayer.Param[i]:=TMPPlayer.Param[i]+Temp*1;
         Tempo:=-1;
        end;
      end;
     for i:=1 to 7 do
      begin
       if TMPPlayer.Param[i]>7 then
        begin
         TMPPlayer.Param[i]:=7;
         flag:=false;
        end;
       if TMPPlayer.Param[i]<0 then
        begin
         TMPPlayer.Param[i]:=0;
         flag:=false;
        end;
      end;
     if flag then TmpRating:=TmpRating-Temp*1;
    end;
  end;
 ParamPlus:=TMPPlayer;
end;

{‘”Õ ÷»ﬂ "Œœ–≈ƒ≈À≈Õ»≈ Œ¡Ÿ≈√Œ –≈…“»Õ√¿  ŒÃ¿Õƒ€"}
function TeamAllRating(Team: TTeam): integer;
var Temp,i: integer;
begin
 Temp:=0;
 for i:=1 to 30 do
  Temp:=Temp+Team.Player[i].Rating;
 TeamAllRating:=Temp;
end;

{‘”Õ ÷»ﬂ "—À”◊¿…ÕŒ≈ Œœ–≈ƒ≈À≈Õ»≈ –¿—€"}
function RandomRasa(Country: integer): integer;
begin
 {??? Õ‡ÈÚË ‡Ò˚ ‰Îˇ Í‡Ê‰ÓÈ ËÁ ÒÚ‡Ì}
 RandomRasa:=Rand(0,2);
// case Country of
// end;
{function Rasa(Amplua:char):char;
var Vyb:longint;
    NumAmp,Smes:longint;
begin
randomize;
if (National=56) or (National=59) or (National=69) or (National=74) or
(National=76) or (National=77) or (National=80) or (National=91) or
(National=105) or (National=115) or (National=118) or (National=119) or
(National=123) or (National=124) or ((National>=126) and (National<=136)) or
((National>=138) and (National<=140)) or (National=142) or
((National>=144) and (National<=147)) then Vyb:=1;{W}
{if (National=62) or (National=63) or (National=67) or (National=75) or
(National=78) or ((National>=82) and (National<=90)) or ((National>=92) and
(National<=104)) or ((National>=106) and (National<=114)) or (National=116) or
(National=117) or (National=121) or (National=122) or (National=125) or
(National=137) or (National=141) or (National=143) or (National=150) or
(National=151) then Vyb:=2;{B}
{if (National=0) or ((National>=2) and (National<=7)) or ((National>=9) and
(National<=11)) or ((National>=13) and (National<=22)) or ((National>=24) and
(National<=26)) or ((National>=28) and (National<=45)) or ((National>=47) and
(National<=50)) or (National=52) or (National=66) or (National=68) or
(National=71) or (National=148) or (National=149) then Vyb:=3;{WY}
{if (National=1) or (National=8) or (National=12) or (National=23) or
(National=27) or (National=46) or (National=51) or ((National>=53) and
(National<=55)) or (National=57) or (National=58) or (National=60) or
(National=61) or (National=64) or (National=65) or (National=70) or
(National=72) or (National=73) or (National=79) or (National=81) or
(National=152) then Vyb:=4;{WYB}
{if (National=120) then Vyb:=5;{WB}
{NumAmp:=ord(c) div 32;Smes:=0;
if Vyb=1 then Smes:=0;
if Vyb=2 then Smes:=16;
if Vyb=3 then Smes:=random(2)*8;
if Vyb=4 then Smes:=random(3)*8;
if Vyb=5 then Smes:=random(2)*16;
NumAmp:=NumAmp*32+Smes;
Rasa:=chr(NumAmp);
end;}
end;

{œ–Œ÷≈ƒ”–¿ "»«Ã≈Õ≈Õ»≈  ŒÃ¿Õƒ"}
procedure TForm1.ChangeTeams;
var i,j: integer;
    Tmp: integer;

procedure ChangeTeam(Nm,Num: integer);
var i,j: integer;
    s: string;
    NumTran: integer;
    TransferList: array of integer;
    Sum: integer;

procedure AddRemovePoints(Team: TTeam);
var Plus,Rating: integer;
    Index: integer;
begin
 Rating:=Rand(29*Team.TeamClass,31*Team.TeamClass);
 Rating:=Rating-TeamAllRating(Team);
 if Rating<0 then begin Rating:=-Rating;Plus:=-1;end
             else Plus:=1;
 while Rating<>0 do
  begin
   Index:=Rand(1,30);
   if Team.Player[Index].Age<26 then Tmp := 0 else
                                     Tmp := Rand(0,1);
   if Tmp=0 then
    begin
     if Team.Player[Index].Rating+Plus<0  then begin Plus:=-Team.Player[Index].Rating;Team.Player[Index].Rating:=0;end else
      if Team.Player[Index].Rating+Plus>49 then begin Plus:=49-Team.Player[Index].Rating;Team.Player[Index].Rating:=49;end else
       begin
        Team.Player[Index].Rating:=Team.Player[Index].Rating+Plus;
        Team.Player[Index]:=ParamPlus(Team.Player[Index],Plus);
        Rating:=Rating-1;
       end;
    end;
  end;
end;

begin
 for i:=1 to 30 do
  if Teams[Nm,Num].Player[i].Flag then
   begin
    Tmp := Rand(-3,3);
    if Teams[Nm,Num].Player[i].Age<21 then Tmp := Tmp + Rand(0,2) else
    if Teams[Nm,Num].Player[i].Age<26 then Tmp := Tmp + Rand(0,1);
    if Tmp>3 then Tmp:=3;
    if Teams[Nm,Num].Player[i].Rating+Tmp<0 then
     begin
      Tmp:=-Teams[Nm,Num].Player[i].Rating;
      Teams[Nm,Num].Player[i].Rating := 0;
     end
    else
    if Teams[Nm,Num].Player[i].Rating+Tmp>49 then
     begin
      Tmp:=49-Teams[Nm,Num].Player[i].Rating;
      Teams[Nm,Num].Player[i].Rating := 49;
     end
    else
     Teams[Nm,Num].Player[i].Rating := Teams[Nm,Num].Player[i].Rating+Tmp;
    Teams[Nm,Num].Player[i]:=ParamPlus(Teams[Nm,Num].Player[i],Tmp);
    Teams[Nm,Num].Player[i].Number:=0;
   end
  else
 begin
  Tmp:=Rand(1,100);
  if Tmp>25 then Teams[Nm,Num].Player[i].Country:=Teams[Nm,Num].Country else
  if (Tmp>5) and Teams[Nm,Num].Trener.Flag then Teams[Nm,Num].Player[i].Country:=Teams[Nm,Num].Trener.Country
                                           else Teams[Nm,Num].Player[i].Country:=Rand(0,152);
  s:=IntToStr(Teams[Nm,Num].Player[i].Country);
  if length(s)=1 then s:='00'+s;
  if length(s)=2 then s:='0'+s;
  ComboBox2.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.nam');
  ComboBox4.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.fam');
  Teams[Nm,Num].Player[i].Name:=ComboBox2.Items.Strings[Rand(0,ComboBox2.Items.Count-1)] + ' ' + ComboBox4.Items.Strings[Rand(0,ComboBox4.Items.Count-1)];
  Teams[Nm,Num].Player[i].Rasa:=RandomRasa(Teams[Nm,Num].Player[i].Country);
  Teams[Nm,Num].Player[i].Rating:=Rand(0,Teams[Nm,Num].TeamClass);
  for j:=1 to 7 do Teams[Nm,Num].Player[i].Param[j]:=0;
  Teams[Nm,Num].Player[i] := ParamPlus(Teams[Nm,Num].Player[i],Teams[Nm,Num].Player[i].Rating);
  Teams[Nm,Num].Player[i].Age:=Rand(16,20);
  Teams[Nm,Num].Player[i].Number:=0;
  Teams[Nm,Num].Player[i].Flag:=true;
 end;
 if Teams[Nm,Num].Trener.Flag then
  begin
   Tmp:=Rand(0,3);
   if Teams[Nm,Num].Trener.Rating+Tmp<0  then Teams[Nm,Num].Trener.Rating:=0  else
   if Teams[Nm,Num].Trener.Rating+Tmp>49 then Teams[Nm,Num].Trener.Rating:=49 else
                                              Teams[Nm,Num].Trener.Rating:=Tmp;
  end
 else
  begin
   NumTran:=0;
   SetLength(TransferList,NumTran);
   for i:=0 to NumTreners-1 do
    if (Treners[i].Rating>Teams[Nm,Num].TeamClass) and
       (Treners[i].Rating<=Teams[Nm,Num].TeamClass+10) then
     begin
      NumTran:=NumTran+1;
      SetLength(TransferList,NumTran);
      TransferList[NumTran-1]:=i;
     end;
   if NumTran>0 then
    begin
     Sum:=Rand(0,NumTran-1);
     i:=TransferList[Sum];
     Teams[Nm,Num].Trener.Name:=Treners[i].Name;
     Teams[Nm,Num].Trener.Age:=Treners[i].Age;
     Teams[Nm,Num].Trener.Country:=Treners[i].Country;
     Teams[Nm,Num].Trener.Rating:=Treners[i].Rating;
     Teams[Nm,Num].Trener.Tactic:=Treners[i].Tactic;
     Teams[Nm,Num].Trener.Flag:=true;
     DeleteTrener(i);
    end
   else
    begin
     Teams[Nm,Num].Trener.Age:=Rand(30,60);
     Tmp:=Rand(1,100);
     if Tmp>25 then Teams[Nm,Num].Trener.Country:=Teams[Nm,Num].Country
               else Teams[Nm,Num].Trener.Country:=Rand(0,152);
     s:=IntToStr(Teams[Nm,Num].Trener.Country);
     if length(s)=1 then s:='00'+s;
     if length(s)=2 then s:='0'+s;
     ComboBox2.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.nam');
     ComboBox4.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.fam');
     Teams[Nm,Num].Trener.Name:=ComboBox2.Items.Strings[Rand(0,ComboBox2.Items.Count-1)] + ' ' + ComboBox4.Items.Strings[Rand(0,ComboBox4.Items.Count-1)];
     Teams[Nm,Num].Trener.Rating:=Rand(0,49);
     Teams[Nm,Num].Trener.Tactic:=0;
     Teams[Nm,Num].Trener.Flag:=true;
    end
  end;
 AddRemovePoints(Teams[Nm,Num]);
end;

begin
 for i:=0 to ComboBox5.Items.Count-1 do
  for j:=0 to NumTeams[i]-1 do
   ChangeTeam(i,j);
 i:=0;
 while i<NumTreners do
  begin
   Tmp := Rand(-3,0);
   if Treners[i].Rating+Tmp<0 then DeleteTrener(i) else i:=i+1;
  end;
 i:=0;
 while i<NumPlayers do
  begin
   Tmp := Rand(-3,0);
   if Players[i].Rating+Tmp<0 then DeletePlayer(i) else begin Players[i].Rating:=Players[i].Rating+Tmp;Players[i]:=ParamPlus(Players[i],Tmp);i:=i+1;end;
  end;
end;

{œ–Œ÷≈ƒ”–¿ "—Œ«ƒ¿Õ»≈ “¿ “»   ŒÃ¿Õƒ"}
procedure TForm1.CreateTactics;
var i,j: integer;

procedure CreateTactic(Nm,Num: integer);
const Tactics: array [0..11,1..3] of byte = ((2,2,2),
                                             (3,2,1),
                                             (2,3,1),
                                             (3,1,2),
                                             (1,3,2),
                                             (2,1,3),
                                             (2,0,4),
                                             (1,2,3),
                                             (2,2,2),
                                             (3,0,3),
                                             (1,0,5),
                                             (4,1,1));
var i,j: integer;
    Amp: array [0..7] of integer;
    Sred: array [0..7] of integer;
    Kol: array [0..7] of integer;
    Kl: array [1..3] of integer;
    Sr: array [1..3] of integer;
    Max: array [1..3] of integer;
    Min: array [1..3] of integer;
    Ready: array [1..3] of boolean;
    sum_pl: integer;
    ko: integer;
    Tact: array [0..11,1..3] of integer;
    TactFlag: array [0..11] of boolean;
    Temp,NumTact: integer;
    flag: boolean;

function FindMax(A,B,C: array of integer): integer;
var Tmp: integer;
    X: array [0..2] of integer;
    i,j: integer;
    kol,max,Temp: integer;
begin
 Tmp:=0;
 for i:=0 to 2 do X[i]:=0;
 for i:=0 to 2 do
  for j:=0 to 2 do
   if (A[i]>A[j]) or ((A[i]=A[j]) and (B[i]>B[j])) then
    X[i]:=X[i]+1;
 for i:=0 to 2 do
  if A[i]<=C[i] then
   X[i]:=-1;
 max:=0;
 kol:=0;
 for i:=0 to 2 do
  if X[i]>max then
   begin
    max:=X[i];
    kol:=1;
   end
  else
   if X[i]=max then kol:=kol+1;
 Temp:=Rand(0,kol-1);
 j:=0;
 for i:=0 to 2 do
  if X[i]=max then
   begin
    if j=Temp then Tmp:=i+1;
    j:=j+1;
   end;
 FindMax:=Tmp;
end;

function FindMin(A,B,C: array of integer): byte;
var Tmp: integer;
    X: array [0..2] of integer;
    i,j: integer;
    kol,max,Temp: integer;
begin
 Tmp:=0;
 for i:=0 to 2 do X[i]:=0;
 for i:=0 to 2 do
  for j:=0 to 2 do
   if (A[i]<A[j]) or ((A[i]=A[j]) and (B[i]>B[j])) then
    X[i]:=X[i]+1;
 for i:=0 to 2 do
  if A[i]>=C[i] then
   X[i]:=-1;
 max:=0;
 kol:=0;
 for i:=0 to 2 do
  if X[i]>max then
   begin
    max:=X[i];
    kol:=1;
   end
  else
   if X[i]=max then kol:=kol+1;
 Temp:=Rand(0,kol-1);
 j:=0;
 for i:=0 to 2 do
  if X[i]=max then
   begin
    if j=Temp then Tmp:=i+1;
    j:=j+1;
   end;
 FindMin:=Tmp;
end;

procedure CreateSostav;
var i,j: integer;
    a,b: integer;

procedure FindTheBest(Amp1,Amp2,Number: byte);
var i,j,k: integer;
    max,sum: integer;
begin
 max:=-1;
 sum:=0;
 for i:=1 to 30 do
  if (Teams[Nm,Num].Player[i].Amplua>=Amp1) and (Teams[Nm,Num].Player[i].Amplua<=Amp2) and (Teams[Nm,Num].Player[i].Number=0) then
   if Teams[Nm,Num].Player[i].Rating>max then
    begin
     max:=Teams[Nm,Num].Player[i].Rating;
     sum:=1;
    end
   else
    if Teams[Nm,Num].Player[i].Rating=max then sum:=sum+1;
 k:=Rand(0,sum-1);
 j:=0;
 for i:=1 to 30 do
  if (Teams[Nm,Num].Player[i].Amplua>=Amp1) and (Teams[Nm,Num].Player[i].Amplua<=Amp2) and
  (Teams[Nm,Num].Player[i].Number=0) and (Teams[Nm,Num].Player[i].Rating=max) then
   begin
    if j=k then Teams[Nm,Num].Player[i].Number:=Number;
    j:=j+1;
   end;
end;

begin
 b:=1; FindTheBest(0,0,b);
 for i:=1 to 2 do
  begin
   b:=b+1; FindTheBest(3*i-2,3*i-2,b);
   a:=Tactics[Teams[Nm,Num].Trener.Tactic,i];
   for j:=1 to a do
    begin
     b:=b+1;
     FindTheBest(3*i,3*i,b);
    end;
   b:=b+1; FindTheBest(3*i-1,3*i-1,b);
  end;
 a:=Tactics[Teams[Nm,Num].Trener.Tactic,3];
 for i:=1 to a do
  begin
   b:=b+1;
   FindTheBest(7,7,b);
  end;
 b:=b+1; FindTheBest(0,0,b);
 b:=b+1; FindTheBest(1,3,b);
 b:=b+1; FindTheBest(4,6,b);
 b:=b+1; FindTheBest(7,7,b);
 b:=b+1; FindTheBest(1,7,b);
end;

procedure SortSostav;
var Reserv: array [1..4] of integer;
    i,j: integer;
    Tmp,Temp,Max: integer;
    Osn: array [2..11] of integer;
begin
 for i:=1 to 30 do
  for j:=13 to 16 do
   if Teams[Nm,Num].Player[i].Number=j then
    Reserv[j-12]:=i;
 for i:=1 to 3 do
  for j:=i+1 to 4 do
   if Teams[Nm,Num].Player[Reserv[i]].Amplua>Teams[Nm,Num].Player[Reserv[j]].Amplua then
    begin
     Tmp:=Teams[Nm,Num].Player[Reserv[i]].Number;
     Teams[Nm,Num].Player[Reserv[i]].Number:=Teams[Nm,Num].Player[Reserv[j]].Number;
     Teams[Nm,Num].Player[Reserv[j]].Number:=Tmp;
     Tmp:=Reserv[i];
     Reserv[i]:=Reserv[j];
     Reserv[j]:=Tmp;
    end;
 case Teams[Nm,Num].Trener.Tactic of
  0: begin
      {—ıÂÏ‡ 4-4-2}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[5]].Number:=5;
      Teams[Nm,Num].Player[Osn[6]].Number:=6;
      Teams[Nm,Num].Player[Osn[9]].Number:=9;
      //«‡˘ËÚÌËÍË
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[3]].Number:=3;
        Teams[Nm,Num].Player[Osn[4]].Number:=4;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[3]].Number:=4;
        Teams[Nm,Num].Player[Osn[4]].Number:=3;
       end;
      //Õ‡Ô‡‰‡˛˘ËÂ
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=10;
        Teams[Nm,Num].Player[Osn[11]].Number:=11;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=11;
        Teams[Nm,Num].Player[Osn[11]].Number:=10;
       end;
      //œÓÎÛÁ‡˘ËÚÌËÍË
      Tmp:=Teams[Nm,Num].Player[Osn[7]].Param[1]+Teams[Nm,Num].Player[Osn[7]].Param[3]+Teams[Nm,Num].Player[Osn[7]].Param[4]+Teams[Nm,Num].Player[Osn[7]].Param[6]+Teams[Nm,Num].Player[Osn[7]].Param[7];
      Temp:=Teams[Nm,Num].Player[Osn[8]].Param[1]+Teams[Nm,Num].Player[Osn[8]].Param[3]+Teams[Nm,Num].Player[Osn[8]].Param[4]+Teams[Nm,Num].Player[Osn[8]].Param[6]+Teams[Nm,Num].Player[Osn[8]].Param[7];
      if Tmp>Temp then
       begin
        Teams[Nm,Num].Player[Osn[7]].Number:=8;
        Teams[Nm,Num].Player[Osn[8]].Number:=7;
       end
      else
       if Tmp<Temp then
        begin
         Teams[Nm,Num].Player[Osn[7]].Number:=7;
         Teams[Nm,Num].Player[Osn[8]].Number:=8;
        end
       else
        begin
         if Teams[Nm,Num].Player[Osn[7]].Rating>=Teams[Nm,Num].Player[Osn[8]].Rating then
          begin
           Teams[Nm,Num].Player[Osn[7]].Number:=8;
           Teams[Nm,Num].Player[Osn[8]].Number:=7;
          end
         else
          begin
           Teams[Nm,Num].Player[Osn[7]].Number:=7;
           Teams[Nm,Num].Player[Osn[8]].Number:=8;
          end;
        end;
     end;
  1: begin
      {—ıÂÏ‡ 5-4-1}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[6]].Number:=5;
      Teams[Nm,Num].Player[Osn[7]].Number:=6;
      Teams[Nm,Num].Player[Osn[10]].Number:=9;
      Teams[Nm,Num].Player[Osn[11]].Number:=11;
      //«‡˘ËÚÌËÍË
      Max:=3;
      Tmp:=Teams[Nm,Num].Player[Max].Param[2]+Teams[Nm,Num].Player[Max].Param[5]+Teams[Nm,Num].Player[Max].Param[6];
      for i:=4 to 5 do
       begin
        Temp:=Teams[Nm,Num].Player[Osn[i]].Param[2]+Teams[Nm,Num].Player[Osn[i]].Param[5]+Teams[Nm,Num].Player[Osn[i]].Param[6];
        if (Tmp<Temp) or ((Tmp=Temp) and (Teams[Nm,Num].Player[Max].Rating<Teams[Nm,Num].Player[Osn[i]].Rating)) then
         begin
          Max:=i;
          Tmp:=Tmp;
         end;
       end;
      Tmp:=Osn[Max];
      Osn[Max]:=Osn[3];
      Osn[3]:=Tmp;
      Teams[Nm,Num].Player[Osn[3]].Number:=3;
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[4]].Number:=4;
        Teams[Nm,Num].Player[Osn[5]].Number:=7;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[4]].Number:=7;
        Teams[Nm,Num].Player[Osn[5]].Number:=4;
       end;
      //œÓÎÛÁ‡˘ËÚÌËÍË
      Tmp:=Teams[Nm,Num].Player[Osn[8]].Param[1]+Teams[Nm,Num].Player[Osn[8]].Param[3]+Teams[Nm,Num].Player[Osn[8]].Param[4]+Teams[Nm,Num].Player[Osn[8]].Param[6]+Teams[Nm,Num].Player[Osn[8]].Param[7];
      Temp:=Teams[Nm,Num].Player[Osn[9]].Param[1]+Teams[Nm,Num].Player[Osn[9]].Param[3]+Teams[Nm,Num].Player[Osn[9]].Param[4]+Teams[Nm,Num].Player[Osn[9]].Param[6]+Teams[Nm,Num].Player[Osn[9]].Param[7];
      if Tmp>Temp then
       begin
        Teams[Nm,Num].Player[Osn[8]].Number:=10;
        Teams[Nm,Num].Player[Osn[9]].Number:=8;
       end
      else
       if Tmp<Temp then
        begin
         Teams[Nm,Num].Player[Osn[8]].Number:=8;
         Teams[Nm,Num].Player[Osn[9]].Number:=10;
        end
       else
        begin
         if Teams[Nm,Num].Player[Osn[8]].Rating>=Teams[Nm,Num].Player[Osn[9]].Rating then
          begin
           Teams[Nm,Num].Player[Osn[8]].Number:=10;
           Teams[Nm,Num].Player[Osn[9]].Number:=8;
          end
         else
          begin
           Teams[Nm,Num].Player[Osn[8]].Number:=8;
           Teams[Nm,Num].Player[Osn[9]].Number:=10;
          end;
        end;
     end;
  2: begin
      {—ıÂÏ‡ 4-5-1}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[5]].Number:=5;
      Teams[Nm,Num].Player[Osn[6]].Number:=6;
      Teams[Nm,Num].Player[Osn[10]].Number:=9;
      Teams[Nm,Num].Player[Osn[11]].Number:=11;
      //«‡˘ËÚÌËÍË
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[3]].Number:=3;
        Teams[Nm,Num].Player[Osn[4]].Number:=4;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[3]].Number:=4;
        Teams[Nm,Num].Player[Osn[4]].Number:=3;
       end;
      //œÓÎÛÁ‡˘ËÚÌËÍË
      Max:=7;
      Tmp:=Teams[Nm,Num].Player[Max].Param[2]+Teams[Nm,Num].Player[Max].Param[5]+Teams[Nm,Num].Player[Max].Param[6];
      for i:=8 to 9 do
       begin
        Temp:=Teams[Nm,Num].Player[Osn[i]].Param[2]+Teams[Nm,Num].Player[Osn[i]].Param[5]+Teams[Nm,Num].Player[Osn[i]].Param[6];
        if (Tmp<Temp) or ((Tmp=Temp) and (Teams[Nm,Num].Player[Max].Rating<Teams[Nm,Num].Player[Osn[i]].Rating)) then
         begin
          Max:=i;
          Tmp:=Tmp;
         end;
       end;
      Tmp:=Osn[Max];
      Osn[Max]:=Osn[7];
      Osn[7]:=Tmp;
      Teams[Nm,Num].Player[Osn[7]].Number:=7;
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[8]].Number:=8;
        Teams[Nm,Num].Player[Osn[9]].Number:=10;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[8]].Number:=10;
        Teams[Nm,Num].Player[Osn[9]].Number:=8;
       end;
     end;
  3: begin
      {—ıÂÏ‡ 5-3-2}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[6]].Number:=5;
      Teams[Nm,Num].Player[Osn[7]].Number:=6;
      Teams[Nm,Num].Player[Osn[9]].Number:=9;
      Teams[Nm,Num].Player[Osn[8]].Number:=8;
      //«‡˘ËÚÌËÍË
      Max:=3;
      Tmp:=Teams[Nm,Num].Player[Max].Param[2]+Teams[Nm,Num].Player[Max].Param[5]+Teams[Nm,Num].Player[Max].Param[6];
      for i:=4 to 5 do
       begin
        Temp:=Teams[Nm,Num].Player[Osn[i]].Param[2]+Teams[Nm,Num].Player[Osn[i]].Param[5]+Teams[Nm,Num].Player[Osn[i]].Param[6];
        if (Tmp<Temp) or ((Tmp=Temp) and (Teams[Nm,Num].Player[Max].Rating<Teams[Nm,Num].Player[Osn[i]].Rating)) then
         begin
          Max:=i;
          Tmp:=Tmp;
         end;
       end;
      Tmp:=Osn[Max];
      Osn[Max]:=Osn[3];
      Osn[3]:=Tmp;
      Teams[Nm,Num].Player[Osn[3]].Number:=3;
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[4]].Number:=4;
        Teams[Nm,Num].Player[Osn[5]].Number:=7;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[4]].Number:=7;
        Teams[Nm,Num].Player[Osn[5]].Number:=4;
       end;
      //Õ‡Ô‡‰‡˛˘ËÂ
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=10;
        Teams[Nm,Num].Player[Osn[11]].Number:=11;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=11;
        Teams[Nm,Num].Player[Osn[11]].Number:=10;
       end;
     end;
  4: begin
      {—ıÂÏ‡ 3-5-2}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[3]].Number:=3;
      Teams[Nm,Num].Player[Osn[4]].Number:=5;
      Teams[Nm,Num].Player[Osn[5]].Number:=6;
      Teams[Nm,Num].Player[Osn[9]].Number:=9;
      //œÓÎÛÁ‡˘ËÚÌËÍË
      Max:=6;
      Tmp:=Teams[Nm,Num].Player[Max].Param[1]+Teams[Nm,Num].Player[Max].Param[3]+Teams[Nm,Num].Player[Max].Param[4]+Teams[Nm,Num].Player[Max].Param[6]+Teams[Nm,Num].Player[Max].Param[7];
      for i:=7 to 8 do
       begin
        Temp:=Teams[Nm,Num].Player[Osn[i]].Param[1]+Teams[Nm,Num].Player[Osn[i]].Param[3]+Teams[Nm,Num].Player[Osn[i]].Param[4]+Teams[Nm,Num].Player[Osn[i]].Param[6]+Teams[Nm,Num].Player[Osn[i]].Param[7];
        if (Tmp<Temp) or ((Tmp=Temp) and (Teams[Nm,Num].Player[Max].Rating<Teams[Nm,Num].Player[Osn[i]].Rating)) then
         begin
          Max:=i;
          Tmp:=Tmp;
         end;
       end;
      Tmp:=Osn[Max];
      Osn[Max]:=Osn[6];
      Osn[6]:=Tmp;
      Teams[Nm,Num].Player[Osn[6]].Number:=8;
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[7]].Number:=4;
        Teams[Nm,Num].Player[Osn[8]].Number:=7;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[7]].Number:=7;
        Teams[Nm,Num].Player[Osn[8]].Number:=4;
       end;
      //Õ‡Ô‡‰‡˛˘ËÂ
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=10;
        Teams[Nm,Num].Player[Osn[11]].Number:=11;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=11;
        Teams[Nm,Num].Player[Osn[11]].Number:=10;
       end;
     end;
{  5: 4,3,3
  6: 4,2,4
  7: 3,4,3}
  8: begin
      {—ıÂÏ‡ SWEEP}
      for j:=2 to 11 do
       for i:=1 to 30 do
        if Teams[Nm,Num].Player[i].Number=j then
         Osn[j]:=i;
      Teams[Nm,Num].Player[Osn[2]].Number:=2;
      Teams[Nm,Num].Player[Osn[5]].Number:=5;
      Teams[Nm,Num].Player[Osn[6]].Number:=6;
      Teams[Nm,Num].Player[Osn[9]].Number:=9;
      //Õ‡Ô‡‰‡˛˘ËÂ
      Tmp:=rand(0,1);
      if Tmp=0 then
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=10;
        Teams[Nm,Num].Player[Osn[11]].Number:=11;
       end
      else
       begin
        Teams[Nm,Num].Player[Osn[10]].Number:=11;
        Teams[Nm,Num].Player[Osn[11]].Number:=10;
       end;
      //«‡˘ËÚÌËÍË
      Tmp:=Teams[Nm,Num].Player[Osn[3]].Param[2]+Teams[Nm,Num].Player[Osn[3]].Param[5]+Teams[Nm,Num].Player[Osn[3]].Param[6];
      if Tmp>Teams[Nm,Num].Player[Osn[4]].Param[2]+Teams[Nm,Num].Player[Osn[4]].Param[5]+Teams[Nm,Num].Player[Osn[4]].Param[6] then
       begin
        Teams[Nm,Num].Player[Osn[3]].Number:=3;
        Teams[Nm,Num].Player[Osn[4]].Number:=4;
       end
      else
       if Tmp<Teams[Nm,Num].Player[Osn[4]].Param[2]+Teams[Nm,Num].Player[Osn[4]].Param[5]+Teams[Nm,Num].Player[Osn[4]].Param[6] then
        begin
         Teams[Nm,Num].Player[Osn[3]].Number:=4;
         Teams[Nm,Num].Player[Osn[4]].Number:=3;
        end
       else
        begin
         if Teams[Nm,Num].Player[Osn[3]].Rating>=Teams[Nm,Num].Player[Osn[4]].Rating then
          begin
           Teams[Nm,Num].Player[Osn[3]].Number:=3;
           Teams[Nm,Num].Player[Osn[4]].Number:=4;
          end
         else
          begin
           Teams[Nm,Num].Player[Osn[3]].Number:=4;
           Teams[Nm,Num].Player[Osn[4]].Number:=3;
          end;
        end;
      //œÓÎÛÁ‡˘ËÚÌËÍË
      Tmp:=Teams[Nm,Num].Player[Osn[7]].Param[1]+Teams[Nm,Num].Player[Osn[7]].Param[3]+Teams[Nm,Num].Player[Osn[7]].Param[4]+Teams[Nm,Num].Player[Osn[7]].Param[6]+Teams[Nm,Num].Player[Osn[7]].Param[7];
      if Tmp>Teams[Nm,Num].Player[Osn[8]].Param[1]+Teams[Nm,Num].Player[Osn[8]].Param[3]+Teams[Nm,Num].Player[Osn[8]].Param[4]+Teams[Nm,Num].Player[Osn[8]].Param[6]+Teams[Nm,Num].Player[Osn[8]].Param[7] then
       begin
        Teams[Nm,Num].Player[Osn[7]].Number:=8;
        Teams[Nm,Num].Player[Osn[8]].Number:=7;
       end
      else
       if Tmp<Teams[Nm,Num].Player[Osn[8]].Param[1]+Teams[Nm,Num].Player[Osn[8]].Param[3]+Teams[Nm,Num].Player[Osn[8]].Param[4]+Teams[Nm,Num].Player[Osn[8]].Param[6]+Teams[Nm,Num].Player[Osn[8]].Param[7] then
        begin
         Teams[Nm,Num].Player[Osn[7]].Number:=7;
         Teams[Nm,Num].Player[Osn[8]].Number:=8;
        end
       else
        begin
         if Teams[Nm,Num].Player[Osn[7]].Rating>=Teams[Nm,Num].Player[Osn[8]].Rating then
          begin
           Teams[Nm,Num].Player[Osn[7]].Number:=8;
           Teams[Nm,Num].Player[Osn[8]].Number:=7;
          end
         else
          begin
           Teams[Nm,Num].Player[Osn[7]].Number:=7;
           Teams[Nm,Num].Player[Osn[8]].Number:=8;
          end;
        end;
     end;
{  9: 5,2,3
  10: 3,2,5
  11: 6,3,1}
 end;
end;

begin
 for i:=0 to 7 do
  begin
   Amp[i]:=0;
   Sred[i]:=0;
   Kol[i]:=0;
  end;
 for i:=1 to 30 do
  begin
   Amp[Teams[Nm,Num].Player[i].Amplua]:=Amp[Teams[Nm,Num].Player[i].Amplua]+1;
   Sred[Teams[Nm,Num].Player[i].Amplua]:=Sred[Teams[Nm,Num].Player[i].Amplua]+Teams[Nm,Num].Player[i].Rating;
  end;
 for i:=0 to 7 do Sred[i]:=Sred[i] div Amp[i];
 for i:=1 to 30 do
  if Teams[Nm,Num].Player[i].Rating>=Sred[Teams[Nm,Num].Player[i].Amplua] then
   Kol[Teams[Nm,Num].Player[i].Amplua]:=Kol[Teams[Nm,Num].Player[i].Amplua]+1;
 Kl[1]:=Kol[3];
 Kl[2]:=Kol[6];
 Kl[3]:=Kol[7];
 Sr[1]:=Sred[3];
 Sr[2]:=Sred[6];
 Sr[3]:=Sred[7];
 Max[1]:=Amp[3];
 if Amp[1]+Amp[2]=2 then Amp[3]:=Amp[3]-1;
 Max[2]:=Amp[6];
 if Amp[4]+Amp[5]=2 then Amp[6]:=Amp[6]-1;
 Max[3]:=Amp[7]-1;
 Min[1]:=1;
 Min[2]:=0;
 Min[3]:=1;
 sum_pl:=0;
 for i:=1 to 3 do
  begin
   sum_pl:=sum_pl+Kl[i];
   Ready[i]:=false;
  end;
 for i:=1 to 3 do
  begin
   Kl[i]:=round(Kl[i]*6/sum_pl);
   if Kl[i]>Max[i] then
    begin
     Ready[i]:=true;
     Kl[i]:=Max[i];
    end;
   if Kl[i]<Min[i] then
    begin
     Ready[i]:=true;
     Kl[i]:=Min[i];
    end;
  end;
 sum_pl:=6;
 for i:=1 to 3 do sum_pl:=sum_pl-Kl[i];
 for i:=1 to sum_pl do
  begin
   j:=FindMin(Kl,Sr,Max);
   Kl[j]:=Kl[j]+1;
  end;
 for i:=-1 downto sum_pl do
  begin
   j:=FindMax(Kl,Sr,Min);
   Kl[j]:=Kl[j]-1;
  end;
 ko:=-1;
 for i:=0 to 11 do
  if (Tactics[i,1]=Kl[1]) and (Tactics[i,2]=Kl[2]) and (Tactics[i,3]=Kl[3]) then
   ko:=i;
 while ko=-1 do
  begin
   for i:=0 to 11 do
    begin
     for j:=1 to 3 do
      Tact[i,j]:=Tactics[i,j]-Kl[j];
     TactFlag[i]:=false;
    end;
   for i:=0 to 11 do
    for j:=1 to 3 do
     if Tact[i,j]=0 then
      TactFlag[i]:=true;
   for i:=0 to 11 do
    if TactFlag[i] then
     begin
      flag:=false;
      for j:=1 to 3 do
       if (Tact[i,j]=-1) or (Tact[i,j]=1) then
        flag:=true;
      TactFlag[i]:=flag;
     end;
   NumTact:=0;
   for i:=0 to 11 do
    if TactFlag[i] then
     NumTact:=NumTact+1;
   Temp:=Rand(0,NumTact-1);
   NumTact:=0;
   for i:=0 to 11 do
    if TactFlag[i] then
     begin
      if Temp=NumTact then
       ko:=i;
      NumTact:=NumTact+1;
     end;
  end;
 if ko=8 then Teams[Nm,Num].Trener.Tactic:=Rand(0,1)*8
         else Teams[Nm,Num].Trener.Tactic:=ko;
 CreateSostav;
 SortSostav;
end;

begin
 for i:=0 to ComboBox5.Items.Count-1 do
  for j:=0 to NumTeams[i]-1 do
   CreateTactic(i,j);
end;

{œ–Œ÷≈ƒ”–¿ "“–¿Õ—‘≈–€"}
procedure TForm1.Transfers;
var i,j,k: integer;
    Tmp: integer;
    Sum: integer;
    OwnFlag: boolean;
    FreeFlag: boolean;
    WorldFlag: boolean;
    NumTran: integer;

procedure TransferTrener(Nm,Num: integer);
var i,j: integer;
    TransferList: array of array [1..2] of integer;
begin
 if Teams[Nm,Num].Trener.Flag then
  begin
   if Teams[Nm,Num].Trener.Rating<=Teams[Nm,Num].TeamClass then Tmp:=Rand(0,1);
   if Tmp=0 then
    begin
     NumTreners:=NumTreners+1;
     SetLength(Treners,NumTreners);
     Treners[NumTreners-1].Name:=Teams[Nm,Num].Trener.Name;
     Treners[NumTreners-1].Age:=Teams[Nm,Num].Trener.Age;
     Treners[NumTreners-1].Country:=Teams[Nm,Num].Trener.Country;
     Treners[NumTreners-1].Rating:=Teams[Nm,Num].Trener.Rating;
     Treners[NumTreners-1].Tactic:=Teams[Nm,Num].Trener.Tactic;
     Treners[NumTreners-1].Flag:=true;
     Teams[Nm,Num].Trener.Flag:=false;
    end;
  end;
 if not Teams[Nm,Num].Trener.Flag then
  begin
   Sum:=Rand(0,3);
   OwnFlag:=false;
   FreeFlag:=false;
   WorldFlag:=false;
   if Sum>=3 then WorldFlag:=true;
   if Sum>=2 then FreeFlag:=true;
   if Sum>=0 then OwnFlag:=true;
   NumTran:=0;
   SetLength(TransferList,NumTran);
   if FreeFlag then
    for i:=0 to NumTreners-1 do
     if (Treners[i].Rating>Teams[Nm,Num].TeamClass) and
        (Treners[i].Rating<=Teams[Nm,Num].TeamClass+10) then
      begin
       NumTran:=NumTran+1;
       SetLength(TransferList,NumTran);
       TransferList[NumTran-1,1]:=-1;
       TransferList[NumTran-1,2]:=i;
      end;
   if WorldFlag then
    begin
     for i:=0 to ComboBox5.Items.Count-1 do
      for j:=0 to NumTeams[i]-1 do
       if (i<>Nm) and (j<>Num) then
        if ((Teams[i,j].Trener.Rating>Teams[Nm,Num].TeamClass) and
            (Teams[i,j].Trener.Rating<=Teams[Nm,Num].TeamClass+10)) and
           ((Teams[i,j].TeamClass<Teams[Nm,Num].TeamClass) or
           ((Teams[i,j].TeamClass>=Teams[Nm,Num].TeamClass) and
            (Teams[i,j].TeamClass>=Teams[i,j].Trener.Rating))) and
            (Teams[i,j].Trener.Flag) then
         begin
          NumTran:=NumTran+1;
          SetLength(TransferList,NumTran);
          TransferList[NumTran-1,1]:=i;
          TransferList[NumTran-1,2]:=j;
         end;
    end
   else
    if OwnFlag then
     begin
      i:=Nm;
      for j:=0 to NumTeams[i]-1 do
       if (i<>Nm) or (j<>Num) then
        if ((Teams[i,j].Trener.Rating>Teams[Nm,Num].TeamClass) and
            (Teams[i,j].Trener.Rating<=Teams[Nm,Num].TeamClass+10)) and
           ((Teams[i,j].TeamClass<Teams[Nm,Num].TeamClass) or
           ((Teams[i,j].TeamClass>=Teams[Nm,Num].TeamClass) and
            (Teams[i,j].TeamClass>=Teams[i,j].Trener.Rating))) and
            (Teams[i,j].Trener.Flag) then
         begin
          NumTran:=NumTran+1;
          SetLength(TransferList,NumTran);
          TransferList[NumTran-1,1]:=i;
          TransferList[NumTran-1,2]:=j;
         end;
     end;
   if NumTran>0 then
    begin
     Sum:=Rand(0,NumTran-1);
     i:=TransferList[Sum,1];
     j:=TransferList[Sum,2];
     if i<0 then
      begin
       Teams[Nm,Num].Trener.Name:=Treners[j].Name;
       Teams[Nm,Num].Trener.Age:=Treners[j].Age;
       Teams[Nm,Num].Trener.Country:=Treners[j].Country;
       Teams[Nm,Num].Trener.Rating:=Treners[j].Rating;
       Teams[Nm,Num].Trener.Tactic:=Treners[j].Tactic;
       Teams[Nm,Num].Trener.Flag:=true;
       DeleteTrener(j);
      end
     else
      begin
       Teams[Nm,Num].Trener.Name:=Teams[i,j].Trener.Name;
       Teams[Nm,Num].Trener.Age:=Teams[i,j].Trener.Age;
       Teams[Nm,Num].Trener.Country:=Teams[i,j].Trener.Country;
       Teams[Nm,Num].Trener.Rating:=Teams[i,j].Trener.Rating;
       Teams[Nm,Num].Trener.Tactic:=Teams[i,j].Trener.Tactic;
       Teams[Nm,Num].Trener.Flag:=true;
       Teams[i,j].Trener.Flag:=false;
      end;
    end;
  end;
end;

procedure TransferPlayer(Nm,Num: integer);
var i,j,k: integer;
    Max: array [0..10] of integer;
    Min: integer;
    TransferList: array of array [1..3] of integer;

procedure DeleteTransfer(Nm,Num,Amp,NumPl: integer);
var i: integer;
    Tmp: integer;
begin
 Tmp:=-1;
 for i:=0 to NumTeamTransfers[Nm,Num,Amp]-1 do
  if TeamTransfers[Nm,Num,Amp,i,2]=NumPl then
   Tmp:=i;
 if Tmp>=0 then
  begin
   for i:=Tmp+1 to NumTeamTransfers[Nm,Num,Amp]-1 do
    TeamTransfers[Nm,Num,Amp,i-1]:=TeamTransfers[Nm,Num,Amp,i];
   NumTeamTransfers[Nm,Num,Amp]:=NumTeamTransfers[Nm,Num,Amp]-1;
   SetLength(TeamTransfers[Nm,Num,Amp],NumTeamTransfers[Nm,Num,Amp]);
  end;
end;

begin
 for i:=0 to 10 do Max[i]:=-1;
 for i:=1 to 30 do
  if Teams[Nm,Num].Player[i].Flag then
   begin
    Tmp:=Teams[Nm,Num].Player[i].Amplua;
    if Teams[Nm,Num].Player[i].Rating>Max[Tmp] then
     Max[Tmp]:=Teams[Nm,Num].Player[i].Rating;
    if Max[8]<Max[3] then
     begin
      Tmp:=Max[8];
      Max[8]:=Max[3];
      Max[3]:=Tmp;
     end;
    if Max[9]<Max[6] then
     begin
      Tmp:=Max[9];
      Max[9]:=Max[6];
      Max[6]:=Tmp;
     end;
    if Max[10]<Max[7] then
     begin
      Tmp:=Max[10];
      Max[10]:=Max[7];
      Max[7]:=Tmp;
     end;
   end;
 Min:=Max[0];
 Sum:=1;
 for i:=1 to 7 do
  if Min>Max[i] then
   begin
    Min:=Max[i];
    Sum:=1;
   end
  else
   if Min=Max[i] then
    Sum:=Sum+1;
 Tmp:=Rand(0,Sum-1);
 Sum:=-1;
 i:=-1;
 j:=-1;
 while Sum<0 do
  begin
   i:=i+1;
   if Max[i]=Min then
    begin
     j:=j+1;
     if j=Tmp then Sum:=i;
    end;
  end;
 Tmp:=-1;
 Min:=0;
 for i:=1 to 30 do
  if Teams[Nm,Num].Player[i].Amplua=Sum then
   begin
    if not Teams[Nm,Num].Player[i].Flag then
     begin
      Min:=-1;
      Tmp:=i;
     end
    else
     if Tmp<0 then
      begin
       Min:=Teams[Nm,Num].Player[i].Rating;
       Tmp:=i;
      end
     else
      if Min>Teams[Nm,Num].Player[i].Rating then
       begin
        Min:=Teams[Nm,Num].Player[i].Rating;
        Tmp:=i;
       end
   end;
 if Teams[Nm,Num].Player[Tmp].Flag then
  begin
   NumPlayers:=NumPlayers+1;
   SetLength(Players,NumPlayers);
   Players[NumPlayers-1].Name:=Teams[Nm,Num].Player[Tmp].Name;
   Players[NumPlayers-1].Age:=Teams[Nm,Num].Player[Tmp].Age;
   Players[NumPlayers-1].Country:=Teams[Nm,Num].Player[Tmp].Country;
   Players[NumPlayers-1].Rating:=Teams[Nm,Num].Player[Tmp].Rating;
   Players[NumPlayers-1].Param:=Teams[Nm,Num].Player[Tmp].Param;
   Players[NumPlayers-1].Amplua:=Teams[Nm,Num].Player[Tmp].Amplua;
   Players[NumPlayers-1].Rasa:=Teams[Nm,Num].Player[Tmp].Rasa;
   Players[NumPlayers-1].Number:=0;
   Players[NumPlayers-1].Flag:=true;
  end;
 Sum:=Rand(0,3);
 OwnFlag:=false;
 FreeFlag:=false;
 WorldFlag:=false;
 if Sum>=3 then WorldFlag:=true;
 if Sum>=2 then FreeFlag:=true;
 if Sum>=0 then OwnFlag:=true;
 NumTran:=0;
 SetLength(TransferList,NumTran);
 if FreeFlag then
  for i:=0 to NumPlayers-1 do
   if (Players[i].Amplua=Teams[Nm,Num].Player[Tmp].Amplua) and
      (Players[i].Rating>Teams[Nm,Num].TeamClass) and
      (Players[i].Rating<=Teams[Nm,Num].TeamClass+10) then
    begin
     NumTran:=NumTran+1;
     SetLength(TransferList,NumTran);
     TransferList[NumTran-1,1]:=-1;
     TransferList[NumTran-1,2]:=-1;
     TransferList[NumTran-1,3]:=i;
    end;
 if WorldFlag then
  begin
   for i:=0 to ComboBox5.Items.Count-1 do
    for j:=0 to NumTeams[i]-1 do
     if (i<>Nm) and (j<>Num) then
      for k:=0 to NumTeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua]-1 do
       if (TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,1]>Teams[Nm,Num].TeamClass) and
          (TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,1]<=Teams[Nm,Num].TeamClass+10) then
        begin
         NumTran:=NumTran+1;
         SetLength(TransferList,NumTran);
         TransferList[NumTran-1,1]:=i;
         TransferList[NumTran-1,2]:=j;
         TransferList[NumTran-1,3]:=TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,2];
        end;
  end
 else
  if OwnFlag then
   begin
    i:=Nm;
    for j:=0 to NumTeams[i]-1 do
     if (i<>Nm) or (j<>Num) then
      for k:=0 to NumTeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua]-1 do
       if (TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,1]>Teams[Nm,Num].TeamClass) and
          (TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,1]<=Teams[Nm,Num].TeamClass+10) then
        begin
         NumTran:=NumTran+1;
         SetLength(TransferList,NumTran);
         TransferList[NumTran-1,1]:=i;
         TransferList[NumTran-1,2]:=j;
         TransferList[NumTran-1,3]:=TeamTransfers[i,j,Teams[Nm,Num].Player[Tmp].Amplua,k,2];
        end;
   end;
 if NumTran>0 then
  begin
   Sum:=Rand(0,NumTran-1);
   i:=TransferList[Sum,1];
   j:=TransferList[Sum,2];
   k:=TransferList[Sum,3];
   if (i<0) or (j<0) then
    begin
     Teams[Nm,Num].Player[Tmp].Name:=Players[k].Name;
     Teams[Nm,Num].Player[Tmp].Age:=Players[k].Age;
     Teams[Nm,Num].Player[Tmp].Country:=Players[k].Country;
     Teams[Nm,Num].Player[Tmp].Rating:=Players[k].Rating;
     Teams[Nm,Num].Player[Tmp].Param:=Players[k].Param;
     Teams[Nm,Num].Player[Tmp].Rasa:=Players[k].Rasa;
     Teams[Nm,Num].Player[Tmp].Number:=0;
     Teams[Nm,Num].Player[Tmp].Flag:=true;
     DeletePlayer(k);
    end
   else
    begin
     Teams[Nm,Num].Player[Tmp].Name:=Teams[i,j].Player[k].Name;
     Teams[Nm,Num].Player[Tmp].Age:=Teams[i,j].Player[k].Age;
     Teams[Nm,Num].Player[Tmp].Country:=Teams[i,j].Player[k].Country;
     Teams[Nm,Num].Player[Tmp].Rating:=Teams[i,j].Player[k].Rating;
     Teams[Nm,Num].Player[Tmp].Param:=Teams[i,j].Player[k].Param;
     Teams[Nm,Num].Player[Tmp].Rasa:=Teams[i,j].Player[k].Rasa;
     Teams[Nm,Num].Player[Tmp].Number:=0;
     Teams[Nm,Num].Player[Tmp].Flag:=true;
     Teams[i,j].Player[k].Flag:=false;
     DeleteTransfer(i,j,Teams[i,j].Player[k].Amplua,k);
    end;
  end;
end;

begin
 for i:=0 to ComboBox5.Items.Count-1 do
  for j:=0 to NumTeams[i]-1 do
   TransferTrener(i,j);
 for i:=0 to ComboBox5.Items.Count-1 do
  for j:=0 to NumTeams[i]-1 do
   begin
    for k:=0 to 7 do
     begin
      NumTeamTransfers[i,j,k]:=0;
      SetLength(TeamTransfers[i,j,k],NumTeamTransfers[i,j,k]);
     end;
    for k:=1 to 30 do
     if ((Teams[i,j].Player[k].Rating>Teams[i,j].TeamClass+3) or
        (Teams[i,j].Player[k].Rating<Teams[i,j].TeamClass-3) or
        (Teams[i,j].Player[k].Age>=35)) and Teams[i,j].Player[k].Flag then
      begin
       Tmp:=Teams[i,j].Player[k].Amplua;
       NumTeamTransfers[i,j,Tmp]:=NumTeamTransfers[i,j,Tmp]+1;
       SetLength(TeamTransfers[i,j,Tmp],NumTeamTransfers[i,j,Tmp]);
       TeamTransfers[i,j,Tmp,NumTeamTransfers[i,j,Tmp]-1,1]:=Teams[i,j].Player[k].Rating;
       TeamTransfers[i,j,Tmp,NumTeamTransfers[i,j,Tmp]-1,2]:=k;
      end;
   end;
 for k:=1 to 5 do
  for i:=0 to ComboBox5.Items.Count-1 do
   for j:=0 to NumTeams[i]-1 do
    TransferPlayer(i,j);
end;

{œ–Œ÷≈ƒ”–¿ "”ƒ¿À≈Õ»≈ ¡≈«–¿¡Œ“ÕŒ√Œ »√–Œ ¿"}
procedure TForm1.DeletePlayer(Num: integer);
var i: integer;
begin
 for i:=Num+1 to NumPlayers-1 do
  Players[i-1]:=Players[i];
 NumPlayers:=NumPlayers-1;
 SetLength(Players,NumPlayers);
end;

{œ–Œ÷≈ƒ”–¿ "”ƒ¿À≈Õ»≈ ¡≈«–¿¡Œ“ÕŒ√Œ “–≈Õ≈–¿"}
procedure TForm1.DeleteTrener(Num: integer);
var i: integer;
begin
 for i:=Num+1 to NumTreners-1 do
  Treners[i-1]:=Treners[i];
 NumTreners:=NumTreners-1;
 SetLength(Treners,NumTreners);
end;

{œ–Œ÷≈ƒ”–¿ "»«Ã≈Õ≈Õ»≈ ¬Œ«–¿—“¿ »√–Œ Œ¬ » “–≈Õ≈–Œ¬"}
procedure TForm1.ChangeAge;
var i,j,k,m: integer;
    Temp,koef: integer;
    s: string;
begin
 i:=0;
 while i<NumTreners do
  begin
   Treners[i].Age:=Treners[i].Age+1;
   if Treners[i].Age>=60 then Temp:=Rand(0,3) else
   if Treners[i].Age>=50 then Temp:=Rand(0,1) else
                              Temp:=0;
   if Temp<>0 then DeleteTrener(i) else i:=i+1;
  end;
 i:=0;
 while i<NumPlayers do
  begin
   Players[i].Age:=Players[i].Age+1;
   if Players[i].Age>=40 then Temp:=Rand(0,3) else
   if Players[i].Age>=35 then Temp:=Rand(0,1) else
                              Temp:=0;
   if Temp<>0 then
    begin
     Temp:=Rand(0,9);
     if Temp=0 then
      begin
       NumTreners:=NumTreners+1;
       SetLength(Treners,NumTreners);
       Treners[NumTreners-1].Name:=Players[i].Name;
       Treners[NumTreners-1].Age:=Players[i].Age;
       Treners[NumTreners-1].Country:=Players[i].Country;
       Treners[NumTreners-1].Rating:=Rand(0,10);
       Treners[NumTreners-1].Tactic:=0;
       Treners[NumTreners-1].Flag:=true;
      end;
     DeletePlayer(i);
    end
   else
    i:=i+1;
  end;
 for k:=0 to ComboBox5.Items.Count-1 do
  for i:=0 to NumTeams[k]-1 do
   begin
    Temp:=Rand(-1,1);
    Teams[k,i].TeamClass:=Teams[k,i].TeamClass+Temp;
    if Teams[k,i].TeamClass<0  then Teams[k,i].TeamClass:=0;
    if Teams[k,i].TeamClass>49 then Teams[k,i].TeamClass:=49;
    Teams[k,i].Trener.Age:=Teams[k,i].Trener.Age+1;
//    if Teams[k,i].Trener.Age>=60 then Temp:=Rand(0,3) else
//    if Teams[k,i].Trener.Age>=50 then Temp:=Rand(0,1) else
//                                      Temp:=0;
//    if Temp<>0 then Teams[k,i].Trener.Flag:=false;

    for j:=1 to 30 do
     begin
      Temp := Rand(1,100);
      if Teams[k,i].Player[j].Age > 29 then
       case Teams[k,i].Player[j].Age of
        30:  if Temp<=10 then Teams[k,i].Player[j].Flag:=false;
        31:  if Temp<=20 then Teams[k,i].Player[j].Flag:=false;
        32:  if Temp<=30 then Teams[k,i].Player[j].Flag:=false;
        33:  if Temp<=40 then Teams[k,i].Player[j].Flag:=false;
        34:  if Temp<=50 then Teams[k,i].Player[j].Flag:=false;
        35:  if Temp<=60 then Teams[k,i].Player[j].Flag:=false;
        36:  if Temp<=70 then Teams[k,i].Player[j].Flag:=false;
        37:  if Temp<=80 then Teams[k,i].Player[j].Flag:=false;
        38:  if Temp<=90 then Teams[k,i].Player[j].Flag:=false;
        39:  if Temp<=95 then Teams[k,i].Player[j].Flag:=false;
        else if Temp<=99 then Teams[k,i].Player[j].Flag:=false;
       end;
      Temp := Rand(-2,2) + Teams[k,i].Player[j].YouthRating;
//      if Teams[k,i].Player[j].Number = 0  then Dec(Temp,3) else
//      if Teams[k,i].Player[j].Number > 11 then Dec(Temp,2);
      if Teams[k,i].Player[j].YouthRating>0 then Dec(Teams[k,i].Player[j].YouthRating);
      Inc(Temp,Teams[k,i].Player[j].Rating);
      if Temp < 0 then
       Teams[k,i].Player[j].Flag := false
      else
       begin
        if Teams[k,i].Player[j].Age > 29 then
         begin
          koef := (Teams[k,i].Player[j].Age - 29);
          if koef>11 then koef:=11;
          koef := koef * (5 + (Teams[k,i].Player[j].Amplua div 4));
          Temp:=(Temp*(100-koef)) div 100;
          if Temp=0 then Teams[k,i].Player[j].Flag := false;
         end;
        koef := (Teams[k,i].Player[j].Age + ((Teams[k,i].Player[j].Amplua + 2) div 3) - 13)*10;
        if Temp > (Teams[k,i].TeamClass * koef) div 100 then
         Temp := (Teams[k,i].TeamClass * koef) div 100;
        if Temp > 49 then Temp:=49;
        Teams[k,i].Player[j]:=ParamPlus(Teams[k,i].Player[j],Temp - Teams[k,i].Player[j].Rating);
        Teams[k,i].Player[j].Rating := Temp;
       end;
      Teams[k,i].Player[j].Age:=Teams[k,i].Player[j].Age+1;
      Teams[k,i].Player[j].Number:=0;
      if not Teams[k,i].Player[j].Flag then
       begin
        Temp:=Rand(0,9);
        if Temp=0 then
         begin
          NumTreners:=NumTreners+1;
          SetLength(Treners,NumTreners);
          Treners[NumTreners-1].Name:=Teams[k,i].Player[j].Name;
          Treners[NumTreners-1].Age:=Teams[k,i].Player[j].Age;
          Treners[NumTreners-1].Country:=Teams[k,i].Player[j].Country;
          Treners[NumTreners-1].Rating:=Rand(0,10);
          Treners[NumTreners-1].Tactic:=0;
          Treners[NumTreners-1].Flag:=true;
         end;
        Temp:=Rand(1,100);
        if Temp>25 then Teams[k,i].Player[j].Country:=Teams[k,i].Country else
        if (Temp>5) and Teams[k,i].Trener.Flag then Teams[k,i].Player[j].Country:=Teams[k,i].Trener.Country
                                               else Teams[k,i].Player[j].Country:=Rand(0,152);
        s:=IntToStr(Teams[k,i].Player[j].Country);
        if length(s)=1 then s:='00'+s;
        if length(s)=2 then s:='0'+s;
        ComboBox2.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.nam');
        ComboBox4.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.fam');
        Teams[k,i].Player[j].Name:=ComboBox2.Items.Strings[Rand(0,ComboBox2.Items.Count-1)] + ' ' + ComboBox4.Items.Strings[Rand(0,ComboBox4.Items.Count-1)];
        Teams[k,i].Player[j].Rasa:=RandomRasa(Teams[k,i].Player[j].Country);
        Teams[k,i].Player[j].Rating:=Rand(0,10);
        for m:=1 to 7 do Teams[k,i].Player[j].Param[m]:=0;
        Teams[k,i].Player[j] := ParamPlus(Teams[k,i].Player[j],Teams[k,i].Player[j].Rating);
        Teams[k,i].Player[j].Age:=Rand(16,19);
        Temp := Rand(0,10) - (Teams[k,i].Player[j].Age - 16);
        if Temp < 0 then Temp := 0;
        Teams[k,i].Player[j].YouthRating := Temp;
        Teams[k,i].Player[j].Number:=0;
        Teams[k,i].Player[j].Flag:=true;
       end;
     end;
   end;
end;

{œ–Œ÷≈ƒ”–¿ "◊“≈Õ»≈ »√–Œ Œ¬"}
procedure TForm1.ReadPlayers;
var i: integer;
begin
 AssignFile(FPlayer, PROGDIR + '\'+IntToStr(Year-1)+'\players.txt');
 Reset(FPlayer);
 NumPlayers:=FileSize(FPlayer);
 SetLength(Players,NumPlayers);
 for i:=0 to NumPlayers-1 do Read(FPlayer,Players[i]);
 CloseFile(FPlayer);
end;

{œ–Œ÷≈ƒ”–¿ "◊“≈Õ»≈ “–≈Õ≈–Œ¬"}
procedure TForm1.ReadTreners;
var i: integer;
begin
 AssignFile(FTrener, PROGDIR + '\'+IntToStr(Year-1)+'\treners.txt');
 Reset(FTrener);
 NumTreners:=FileSize(FTrener);
 SetLength(Treners,NumTreners);
 for i:=0 to NumTreners-1 do Read(FTrener,Treners[i]);
 CloseFile(FTrener);
end;

{œ–Œ÷≈ƒ”–¿ "◊“≈Õ»≈  ŒÃ¿Õƒ"}
procedure TForm1.ReadTeams;
var i,j: integer;
    s: string;
begin
 SetLength(Teams,ComboBox5.Items.Count);
 SetLength(NumTeams,ComboBox5.Items.Count);
 SetLength(TeamTransfers,ComboBox5.Items.Count);
 SetLength(NumTeamTransfers,ComboBox5.Items.Count);
 for i:=0 to ComboBox5.Items.Count-1 do
  begin
   s:=ComboBox5.Items.Strings[i];
   if length(s)=1 then s:='00'+s;
   if length(s)=2 then s:='0'+s;
   AssignFile(FTeam, PROGDIR + '\'+IntToStr(Year-1)+'\'+s+'.txt');
   Reset(FTeam);
   NumTeams[i]:=FileSize(FTeam);
   SetLength(Teams[i],NumTeams[i]);
   SetLength(TeamTransfers[i],NumTeams[i]);
   SetLength(NumTeamTransfers[i],NumTeams[i]);
   for j:=0 to NumTeams[i]-1 do Read(FTeam,Teams[i,j]);
   CloseFile(FTeam);
  end;
end;

{œ–Œ÷≈ƒ”–¿ "«¿œ»—‹ »√–Œ Œ¬"}
procedure TForm1.WritePlayers;
var i: integer;
begin
 CreateDirectory(PChar(PROGDIR + '\'+IntToStr(Year)), NIL);
 AssignFile(FPlayer, PROGDIR + '\'+IntToStr(Year)+'\players.txt');
 Rewrite(FPlayer);
 for i:=0 to NumPlayers-1 do Write(FPlayer,Players[i]);
 CloseFile(FPlayer);
end;

{œ–Œ÷≈ƒ”–¿ "«¿œ»—‹ “–≈Õ≈–Œ¬"}
procedure TForm1.WriteTreners;
var i: integer;
begin
 CreateDirectory(PChar(PROGDIR + '\'+IntToStr(Year)), NIL);
 AssignFile(FTrener, PROGDIR + '\'+IntToStr(Year)+'\treners.txt');
 Rewrite(FTrener);
 for i:=0 to NumTreners-1 do Write(FTrener,Treners[i]);
 CloseFile(FTrener);
end;

{œ–Œ÷≈ƒ”–¿ "«¿œ»—‹  ŒÃ¿Õƒ"}
procedure TForm1.WriteTeams;
var i,j: integer;
    s: string;
begin
 CreateDirectory(PChar(PROGDIR + '\'+IntToStr(Year)), NIL);
 for i:=0 to ComboBox5.Items.Count-1 do
  begin
   s:=ComboBox5.Items.Strings[i];
   if length(s)=1 then s:='00'+s;
   if length(s)=2 then s:='0'+s;
   AssignFile(FTeam, PROGDIR + '\'+IntToStr(Year)+'\'+s+'.txt');
   Rewrite(FTeam);
   for j:=0 to NumTeams[i]-1 do Write(FTeam,Teams[i,j]);
   CloseFile(FTeam);
  end;
end;

{‘”Õ ÷»ﬂ "Œœ–≈ƒ≈À≈Õ»≈ Õ≈Œ¡’Œƒ»ÃŒ√Œ  ŒÃ¿Õƒ≈ ¿ÃœÀ”¿"}
function NeededAmplua(Team: TTeam): integer;
const Ch: array [0..7] of integer = (3,3,3,5,3,3,5,5);
      LimU: array [0..7] of integer = (3,3,3,5,3,3,5,5);
      LimD: array [0..7] of integer = (2,1,1,2,1,1,2,2);

var Level: array [0..7] of integer;
    Amp: array [0..7] of integer;
    Sred: array [0..7] of integer;
    Temp,Tmp,Tempo: integer;
    NumPl: integer;
    flag: boolean;
    SumL: integer;
    i: integer;

function KontrSum: byte;
var Temp: byte;
    i: integer;
begin
 Temp:=0;
 for i:=0 to 7 do if Amp[i]<LimD[i] then Temp:=Temp+LimD[i]-Amp[i];
 KontrSum:=Temp;
end;

begin
 {??? œÓ‚ÂËÚ¸ ‡·ÓÚÛ Ë ‚ÓÁÏÓÊÌÓ ÛÎÛ˜¯ËÚ¸}
 for i:=0 to 7 do
  begin
   Amp[i]:=0;
   Sred[i]:=0;
  end;
 NumPl:=0;
 for i:=1 to 30 do
  if Team.Player[i].Flag then
   begin
    Amp[Team.Player[i].Amplua]:=Amp[Team.Player[i].Amplua]+1;
    Sred[Team.Player[i].Amplua]:=Sred[Team.Player[i].Amplua]+Team.Player[i].Rating;
    NumPl:=NumPl+1;
   end;
 for i:=0 to 7 do
  if Amp[i]<>0 then
   Sred[i]:=Sred[i] div Amp[i];
 Temp:=0;
 for i:=0 to 7 do
  if Sred[i]>Temp then
   Temp:=Sred[i];
 SumL:=0;
 for i:=0 to 7 do
  begin
   Level[i]:=Ch[i]-Amp[i];
   if Level[i]<1 then Level[i]:=1;
   Level[i]:=Level[i]+(Temp-Sred[i]) div 10;
   if Amp[i]=LimU[i] then Level[i]:=0;
   SumL:=SumL+Level[i];
  end;
 repeat
  flag:=true;
  Temp:=Rand(0,SumL-1);
  Tmp:=0;
  Tempo:=0;
  for i:=0 to 7 do
   begin
    Tmp:=Tmp+Level[i];
    if (Temp>=SumL-Tmp) and (Temp<>-1) then
     begin
      Tempo:=i;
      Temp:=-1;
     end;
   end;
  if (Amp[Tempo]>=LimD[Tempo]) and (31-NumPl=KontrSum) then flag:=false;
 until flag;
 NeededAmplua:=Tempo;
end;

{œ–Œ÷≈ƒ”–¿ "◊“≈Õ»≈ Œ–»√»Õ¿À‹Õ€’  ŒÃ¿Õƒ » —Œ«ƒ¿Õ»≈ Õ¿ »’ Œ—ÕŒ¬≈ —¬Œ»’"}
procedure TForm1.ReadOriginalTeams;
var i,j: integer;
    s: string;

procedure ReadOriginalTeam(Nm,Num: integer);
var i,j: integer;
    Temp: integer;

procedure ReadOriginalTrener;
var i: integer;
begin
 Teams[Nm,Num].Trener.Name:='';
 for i:=1 to 24 do
  begin
   Seek(FSWS,Num*684+i+37);
   Read(FSWS,c);
   Teams[Nm,Num].Trener.Name:=Teams[Nm,Num].Trener.Name+CHR(c);
  end;
 Seek(FSWS,Num*684+26);
 Read(FSWS,c);
 Teams[Nm,Num].Trener.Tactic:=c;
 Teams[Nm,Num].Trener.Age:=Rand(30,60);
 Temp:=Rand(1,100);
 if Temp>25 then Teams[Nm,Num].Trener.Country:=Teams[Nm,Num].Country
            else Teams[Nm,Num].Trener.Country:=Rand(0,152);
 Teams[Nm,Num].Trener.Rating:=Rand(0,49);
 Teams[Nm,Num].Trener.Flag:=true;
end;

procedure ReadOriginalPlayer(NumPl: integer);
var i: integer;
    YearBegin,TeamClass: byte;
    Youth: integer;
begin
 Teams[Nm,Num].Player[NumPl].Name:='';
 for i:=1 to 22 do
  begin
   Seek(FSWS,Num*684+NumPl*38+i+42);
   Read(FSWS,c);
   Teams[Nm,Num].Player[NumPl].Name:=Teams[Nm,Num].Player[NumPl].Name+CHR(c);
  end;
 Seek(FSWS,Num*684+NumPl*38+40);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Country:=c;
 Seek(FSWS,Num*684+NumPl*38+72);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Rating:=c;
 Seek(FSWS,Num*684+NumPl*38+42);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Number:=c;
 Seek(FSWS,Num*684+NumPl*38+66);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Amplua:=c div 32;
 Teams[Nm,Num].Player[NumPl].Rasa:=(c mod 32) div 8;
 Seek(FSWS,Num*684+NumPl*38+68);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Param[1]:=c mod 8;
 Seek(FSWS,Num*684+NumPl*38+69);
 Read(FSWS,c);
 if Teams[Nm,Num].Player[NumPl].Amplua=3 then
  begin
   Teams[Nm,Num].Player[NumPl].Param[3]:=(c div 16) mod 8;
   Teams[Nm,Num].Player[NumPl].Param[5]:=(c mod 16) mod 8;
  end
 else
  begin
   Teams[Nm,Num].Player[NumPl].Param[3]:=(c div 16) mod 8;
   Teams[Nm,Num].Player[NumPl].Param[2]:=(c mod 16) mod 8;
  end;
 Seek(FSWS,Num*684+NumPl*38+70);
 Read(FSWS,c);
 if Teams[Nm,Num].Player[NumPl].Amplua=3 then
  begin
   Teams[Nm,Num].Player[NumPl].Param[2]:=(c div 16) mod 8;
   Teams[Nm,Num].Player[NumPl].Param[4]:=(c mod 16) mod 8;
  end
 else
  begin
   Teams[Nm,Num].Player[NumPl].Param[5]:=(c div 16) mod 8;
   Teams[Nm,Num].Player[NumPl].Param[4]:=(c mod 16) mod 8;
  end;
 Seek(FSWS,Num*684+NumPl*38+71);
 Read(FSWS,c);
 Teams[Nm,Num].Player[NumPl].Param[6]:=(c div 16) mod 8;
 Teams[Nm,Num].Player[NumPl].Param[7]:=(c mod 16) mod 8;
 TeamClass := Teams[Nm,Num].TeamClass;
 if TeamClass = 0 then
  TeamClass := 1;
 if (Teams[Nm,Num].Player[NumPl].Rating*10) mod TeamClass = 0 then
  YearBegin := (Teams[Nm,Num].Player[NumPl].Rating*10) div TeamClass
 else
  YearBegin := ((Teams[Nm,Num].Player[NumPl].Rating*10) div TeamClass) + 1;
 if YearBegin>20 then YearBegin := 20;
 YearBegin := YearBegin + 13 - ((Teams[Nm,Num].Player[NumPl].Amplua + 2) div 3);
 if YearBegin < 16 then YearBegin := 16;
 if YearBegin > 30 then YearBegin := 30;
 Teams[Nm,Num].Player[NumPl].Age:=Rand(YearBegin,30);
 Youth := Rand(0,10) - (Teams[Nm,Num].Player[NumPl].Age - 16);
 if Youth < 0 then Youth := 0;
 Teams[Nm,Num].Player[NumPl].YouthRating := Youth;
 Teams[Nm,Num].Player[NumPl].Flag:=true;
end;

procedure CreateYouthPlayer(NumPl: integer);
var i: integer;
    s: AnsiString;
    Youth: integer;
begin
 Temp:=Rand(1,100);
 if Temp>25 then Teams[Nm,Num].Player[NumPl].Country:=Teams[Nm,Num].Country else
 if (Temp>5) and Teams[Nm,Num].Trener.Flag then Teams[Nm,Num].Player[NumPl].Country:=Teams[Nm,Num].Trener.Country
                                           else Teams[Nm,Num].Player[NumPl].Country:=Rand(0,152);
 s:=IntToStr(Teams[Nm,Num].Player[NumPl].Country);
 if length(s)=1 then s:='00'+s;
 if length(s)=2 then s:='0'+s;
 ComboBox2.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.nam');
 ComboBox4.Items.LoadFromFile(PROGDIR + '\NAMEFAM\' + s + '.fam');
 Teams[Nm,Num].Player[NumPl].Name:=ComboBox2.Items.Strings[Rand(0,ComboBox2.Items.Count-1)] + ' ' + ComboBox4.Items.Strings[Rand(0,ComboBox4.Items.Count-1)];
 Teams[Nm,Num].Player[NumPl].Rasa:=RandomRasa(Teams[Nm,Num].Player[NumPl].Country);
 Teams[Nm,Num].Player[NumPl].Amplua:=NeededAmplua(Teams[Nm,Num]);
 Teams[Nm,Num].Player[NumPl].Rating:=Rand(0,10);
 for i:=1 to 7 do Teams[Nm,Num].Player[NumPl].Param[i]:=0;
 Teams[Nm,Num].Player[NumPl] := ParamPlus(Teams[Nm,Num].Player[NumPl],Teams[Nm,Num].Player[NumPl].Rating);
 Teams[Nm,Num].Player[NumPl].Number:=0;
 Teams[Nm,Num].Player[NumPl].Age:=Rand(16,19);
 Youth := Rand(0,10) - (Teams[Nm,Num].Player[NumPl].Age - 16);
 if Youth < 0 then Youth := 0;
 Teams[Nm,Num].Player[NumPl].YouthRating := Youth;
 Teams[Nm,Num].Player[NumPl].Flag:=true;
end;

begin
 Teams[Nm,Num].Name:='';
 for i:=1 to 18 do
  begin
   Seek(FSWS,Num*684+i+6);
   Read(FSWS,c);
   Teams[Nm,Num].Name:=Teams[Nm,Num].Name+CHR(c);
  end;
 Seek(FSWS,Num*684+2);
 Read(FSWS,c);
 Teams[Nm,Num].Country:=StrToInt(ComboBox3.Items.Strings[c]);
 for i:=1 to 2 do
  for j:=1 to 5 do
   begin
    Seek(FSWS,Num*684+i*5+j+22);
    Read(FSWS,c);
    Teams[Nm,Num].Forma[i,j]:=c;
   end;
 ReadOriginalTrener;
 Teams[Nm,Num].TeamClass := 0;
 for i:=1 to 16 do
  begin
   Seek(FSWS,Num*684+i*38+72);
   Read(FSWS,c);
   Teams[Nm,Num].TeamClass := Teams[Nm,Num].TeamClass + c;
  end;
 Teams[Nm,Num].TeamClass := Teams[Nm,Num].TeamClass div 16;
 if Teams[Nm,Num].TeamClass<0 then Teams[Nm,Num].TeamClass:=0;
 for i:=1 to 16 do ReadOriginalPlayer(i);
 for i:=17 to 30 do CreateYouthPlayer(i);
end;

begin
 SetLength(Teams,ComboBox5.Items.Count);
 SetLength(NumTeams,ComboBox5.Items.Count);
 ProgressBar1.Visible := True;
 ProgressBar1.Max := ComboBox5.Items.Count;
 for i:=0 to ComboBox5.Items.Count-1 do
  begin
   ProgressBar1.Position := i;
   s:=ComboBox5.Items.Strings[i];
   if length(s)=1 then s:='00'+s;
   if length(s)=2 then s:='0'+s;
   AssignFile(FSWS, lblSwosPath.Caption + '\data\team.' + s);
   {$I-}
   Reset(FSWS);
   {$I+}
   if IOResult=0 then
    begin
     Seek(FSWS,1);
     Read(FSWS,c);
     NumTeams[i]:=c;
     SetLength(Teams[i],NumTeams[i]);
     for j:=0 to NumTeams[i]-1 do ReadOriginalTeam(i,j);
     CloseFile(FSWS);
    end;
  end;
 ProgressBar1.Position := ComboBox5.Items.Count;
 ProgressBar1.Visible := False;
end;

procedure TForm1.ComboBox11Change(Sender: TObject);
var i: integer;
begin
 if Length(NumTeams) = 0 then
 begin
   Year := Year + 1;
   ReadTeams;
   Year := Year - 1;
 end;
 ComboBox12.Items.Clear;
 for i:=0 to NumTeams[ComboBox11.ItemIndex]-1 do
  ComboBox12.Items.Add(Teams[ComboBox11.ItemIndex,i].Name);
end;

procedure TForm1.Button3Click(Sender: TObject);
var b,i: integer;
    NC,NT: integer;
begin
 OpenDialog1.InitialDir := PROGDIR;
 if OpenDialog1.Execute then
  begin
   AssignFile(FSWS, OpenDialog1.FileName);
   {$I-}
   Reset(FSWS);
   {$I+}
   if IOResult=0 then
    begin
     b:=55555;
     NC:=ComboBox11.ItemIndex;
     NT:=ComboBox12.ItemIndex;
     c:=StrToInt(ComboBox5.Items.Strings[NC]);
     Seek(FSWS,b+1);
     Write(FSWS,c);
     c:=NT;
     Seek(FSWS,b+2);
     Write(FSWS,c);
     for i:=1 to 18 do
      begin
       c:=ORD(Teams[NC,NT].Name[i]);
       if i>length(Teams[NC,NT].Name) then c:=0;
       Seek(FSWS,b+i+2);
       Write(FSWS,c);
      end;
     CloseFile(FSWS);
    end;
  end;
end;

procedure TForm1.lstSwosPathChange(Sender: TObject);
begin
 lblSwosPath.Caption := lstSwosPath.Directory;
end;

procedure TForm1.btnCareerClick(Sender: TObject);

procedure CreateCareer;
begin
 ReadOriginalTeams;
 NumPlayers:=0;
 NumTreners:=0;
 WritePlayers;
 WriteTreners;
 WriteTeams;
end;

procedure ChangeCareer;
begin
 ReadPlayers;
 ReadTreners;
 ReadTeams;
 ChangeAge;
 Transfers;
 ChangeTeams;
 CreateTactics;
 WritePlayers;
 WriteTreners;
 WriteTeams;
 RecordCustoms;
end;

var ini: TIniFile;

begin
 Year := Year + 1;

 if Year=1 then CreateCareer
           else ChangeCareer;

 Caption := 'SWOS Career Mod. Year: ' + IntToStr(Year);

 ini := TIniFile.Create(PROGDIR + '\Career.ini');
 try
  ini.WriteInteger('Main', 'Year', Year);
 finally
  ini.Free;
 end;
end;

procedure TForm1.FormClose(Sender: TObject; var Action: TCloseAction);
var ini: TIniFile;
begin
  ini := TIniFile.Create(PROGDIR + '\Career.ini');
  try
    ini.WriteString('Main', 'SwosPath', lblSwosPath.Caption);
  finally
    ini.Free;
  end;
end;

procedure TForm1.btnRecordTeamToCareerClick(Sender: TObject);
var b,i,j: integer;
    NC,NT: integer;
begin
 OpenDialog1.InitialDir := PROGDIR;
 if OpenDialog1.Execute then
  begin
   AssignFile(FSWS, OpenDialog1.FileName);
   {$I-}
   Reset(FSWS);
   {$I+}
   if IOResult=0 then
    begin
     b:=56201;
     NC:=ComboBox11.ItemIndex;
     NT:=ComboBox12.ItemIndex;
//     Seek(FSWS,b-1076);
//     Read(FSWS,c);
//     National:=c;
//     Caption:=IntToStr(National);
     {RecordTeam}
{     for i:=1 to 18 do
      begin
       c:=ORD(Teams[Nm,Num].Name[i]);
       if i>length(Teams[Nm,Num].Name) then c:=0;
       Seek(FSWS,Num*684+i+6);
       Write(FSWS,c);
      end;
     c:=StrToInt(ComboBox1.Items.Strings[Teams[Nm,Num].Country]);
     Seek(FSWS,Num*684+2);
     Write(FSWS,c);
     for i:=1 to 2 do
      for j:=1 to 5 do
       begin
        c:=Teams[Nm,Num].Forma[i,j];
        Seek(FSWS,Num*684+i*5+j+22);
        Write(FSWS,c);
       end;}
     {RecordTrener;}
{     for i:=1 to 24 do
      begin
       c:=ORD(Teams[Nm,Num].Trener.Name[i]);
       if i>length(Teams[Nm,Num].Trener.Name) then c:=0;
       Seek(FSWS,Num*684+i+37);
       Write(FSWS,c);
      end;
     c:=Teams[Nm,Num].Trener.Tactic;
     Seek(FSWS,Num*684+26);
     Write(FSWS,c);
     for i:=0 to 15 do
      begin
       c:=i;
       Seek(FSWS,Num*684+i+62);
       Write(FSWS,c);
      end;}
     {RecordPlayer;}
     for i:=1 to 30 do
      begin
       for j:=1 to 22 do
        begin
         c:=ORD(Teams[NC,NT].Player[i].Name[j]);
         if j>length(Teams[NC,NT].Player[i].Name) then c:=0;
         Seek(FSWS,b+i*38+j-1);
         Write(FSWS,c);
        end;
       c:=Teams[NC,NT].Player[i].Country;
       Seek(FSWS,b+i*38-3);
       Write(FSWS,c);
       c:=i;
       Seek(FSWS,b+i*38-1);
       Write(FSWS,c);
       c:=Teams[NC,NT].Player[i].Rating;
       Seek(FSWS,b+i*38+29);
       Write(FSWS,c);
       c:=Teams[NC,NT].Player[i].Amplua*32 + Teams[NC,NT].Player[i].Rasa*8;
       Seek(FSWS,b+i*38+23);
       Write(FSWS,c);
       c:=8+Teams[NC,NT].Player[i].Param[1];
       Seek(FSWS,b+i*38+25);
       Write(FSWS,c);
       if Teams[NC,NT].Player[i].Amplua=3 then c:=(8+Teams[NC,NT].Player[i].Param[3])*16 + 8+Teams[NC,NT].Player[i].Param[5]
                                          else c:=(8+Teams[NC,NT].Player[i].Param[3])*16 + 8+Teams[NC,NT].Player[i].Param[2];
       Seek(FSWS,b+i*38+26);
       Write(FSWS,c);
       if Teams[NC,NT].Player[i].Amplua=3 then c:=(8+Teams[NC,NT].Player[i].Param[2])*16 + 8+Teams[NC,NT].Player[i].Param[4]
                                          else c:=(8+Teams[NC,NT].Player[i].Param[5])*16 + 8+Teams[NC,NT].Player[i].Param[4];
       Seek(FSWS,b+i*38+27);
       Write(FSWS,c);
       c:=(8+Teams[NC,NT].Player[i].Param[6])*16 + 8+Teams[NC,NT].Player[i].Param[7];
       Seek(FSWS,b+i*38+28);
       Write(FSWS,c);
       c:=0;
       Seek(FSWS,b+i*38-2);
       Write(FSWS,c);
       for j:=30 to 34 do
        begin
         Seek(FSWS,b+i*38+j);
         Write(FSWS,c);
        end;
      end;
     c:=30;
     Seek(FSWS,b+30*38+111);
     Write(FSWS,c);
     for i:=0 to 29 do
      begin
       c:=i;
       Seek(FSWS,b-73+i);
       Write(FSWS,c);
      end;
     CloseFile(FSWS);
    end;
  end;
end;

end.
