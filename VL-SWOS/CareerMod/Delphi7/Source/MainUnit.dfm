object Form1: TForm1
  Left = 191
  Top = 114
  Width = 764
  Height = 376
  Caption = 'SWOS Career Mod'
  Color = clBtnFace
  Font.Charset = DEFAULT_CHARSET
  Font.Color = clWindowText
  Font.Height = -11
  Font.Name = 'MS Sans Serif'
  Font.Style = []
  OldCreateOrder = False
  OnClose = FormClose
  OnCreate = FormCreate
  PixelsPerInch = 96
  TextHeight = 13
  object lblSwosPath: TLabel
    Left = 112
    Top = 8
    Width = 87
    Height = 20
    Caption = 'lblSwosPath'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ParentFont = False
  end
  object lblSwosPathCaption: TLabel
    Left = 8
    Top = 8
    Width = 101
    Height = 20
    Caption = 'SWOS Path:'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Label1: TLabel
    Left = 392
    Top = 88
    Width = 122
    Height = 20
    Caption = 'File TEAM.xxx: '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object Label2: TLabel
    Left = 392
    Top = 112
    Width = 125
    Height = 20
    Caption = 'Choose TEAM: '
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
  end
  object ComboBox1: TComboBox
    Left = 72
    Top = 96
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 0
    Text = 'ComboBox1'
    Visible = False
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '17'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '33'
      '34'
      '36'
      '38'
      '39'
      '40'
      '41'
      '76'
      '78'
      '35'
      '72'
      '72'
      '72'
      '72'
      '37'
      '18'
      '72'
      '72'
      '72'
      '72'
      '72'
      '51'
      '72'
      '72'
      '72'
      '60'
      '72'
      '73'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '51'
      '72'
      '43'
      '45'
      '46'
      '48'
      '49'
      '50'
      '64'
      '66'
      '71'
      '77'
      '72'
      '65'
      '42'
      '69'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '79'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '55'
      '67'
      '75'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '72'
      '44'
      '62'
      '72'
      '72'
      '72')
  end
  object ComboBox2: TComboBox
    Left = 72
    Top = 144
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 1
    Text = 'ComboBox2'
    Visible = False
  end
  object ComboBox7: TComboBox
    Left = 224
    Top = 144
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 2
    Text = 'ComboBox7'
    Visible = False
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '9'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '33'
      '34'
      '35'
      '36'
      '37'
      '38'
      '39'
      '40'
      '42'
      '45'
      '46'
      '51'
      '52'
      '54'
      '56'
      '58'
      '68'
      '69'
      '70'
      '71'
      '72'
      '73'
      '74'
      '75'
      '76'
      '77'
      '79'
      '80'
      '81'
      '88'
      '118'
      '119'
      '120'
      '148'
      '149')
  end
  object ComboBox8: TComboBox
    Left = 72
    Top = 192
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 3
    Text = 'ComboBox8'
    Visible = False
    Items.Strings = (
      #1040#1083#1073#1072#1085#1080#1103
      #1040#1074#1089#1090#1088#1080#1103
      #1041#1077#1083#1100#1075#1080#1103
      #1041#1086#1083#1075#1072#1088#1080#1103
      #1061#1086#1088#1074#1072#1090#1080#1103
      #1050#1080#1087#1088
      #1063#1077#1093#1080#1103
      #1044#1072#1085#1080#1103
      #1040#1085#1075#1083#1080#1103
      #1069#1089#1090#1086#1085#1080#1103
      #1060#1072#1088#1077#1088#1099
      #1060#1080#1085#1083#1103#1085#1076#1080#1103
      #1060#1088#1072#1085#1094#1080#1103
      #1043#1077#1088#1084#1072#1085#1080#1103
      #1043#1088#1077#1094#1080#1103
      #1042#1077#1085#1075#1088#1080#1103
      #1048#1089#1083#1072#1085#1076#1080#1103
      #1048#1079#1088#1072#1077#1083#1100
      #1048#1090#1072#1083#1080#1103
      #1051#1072#1090#1074#1080#1103
      #1051#1080#1090#1074#1072
      #1051#1102#1082#1089#1077#1084#1073#1091#1088#1075
      #1052#1072#1083#1100#1090#1072
      #1043#1086#1083#1083#1072#1085#1076#1080#1103
      #1057#1077#1074'.'#1048#1088#1083#1072#1085#1076#1080#1103
      #1053#1086#1088#1074#1077#1075#1080#1103
      #1055#1086#1083#1100#1096#1072
      #1055#1086#1088#1090#1091#1075#1072#1083#1080#1103
      #1056#1091#1084#1099#1085#1080#1103
      #1056#1086#1089#1089#1080#1103
      #1057#1072#1085'-'#1052#1072#1088#1080#1085#1086
      #1064#1086#1090#1083#1072#1085#1076#1080#1103
      #1057#1083#1086#1074#1077#1085#1080#1103
      #1064#1074#1077#1094#1080#1103
      #1058#1091#1088#1094#1080#1103
      #1059#1082#1088#1072#1080#1085#1072
      #1059#1101#1083#1100#1089
      #1070#1075#1086#1089#1083#1072#1074#1080#1103
      #1041#1077#1083#1072#1088#1091#1089#1089#1080#1103
      #1057#1083#1086#1074#1072#1082#1080#1103
      #1048#1089#1087#1072#1085#1080#1103
      #1040#1088#1084#1077#1085#1080#1103
      #1041#1086#1089#1085#1080#1103
      #1040#1079#1077#1088#1073#1072#1081#1076#1078#1072#1085
      #1043#1088#1091#1079#1080#1103
      #1064#1074#1077#1081#1094#1072#1088#1080#1103
      #1048#1088#1083#1072#1085#1076#1080#1103
      #1052#1072#1082#1077#1076#1086#1085#1080#1103
      #1058#1091#1088#1082#1084#1077#1085#1080#1089#1090#1072#1085
      #1051#1080#1093#1090#1080#1085#1096#1090#1077#1081#1085
      #1052#1086#1083#1076#1086#1074#1072
      #1050#1086#1089#1090#1072'-'#1056#1080#1082#1072
      #1057#1072#1083#1100#1074#1072#1076#1086#1088
      #1043#1074#1072#1090#1077#1084#1072#1083#1072
      #1043#1086#1085#1076#1091#1088#1072#1089'-'#1043#1086#1085#1075'-'#1050#1086#1085#1075
      #1041#1072#1075#1072#1084#1099
      #1052#1077#1082#1089#1080#1082#1072
      #1055#1072#1085#1072#1084#1072
      #1057#1064#1040
      #1041#1072#1093#1088#1077#1081#1085
      #1053#1080#1082#1072#1088#1072#1075#1091#1072
      #1041#1077#1088#1084#1091#1076#1072
      #1071#1084#1072#1081#1082#1072
      #1058#1088#1080#1085#1080#1076#1072#1076' '#1080' '#1058#1086#1073#1072#1075#1086
      #1050#1072#1085#1072#1076#1072
      #1041#1072#1088#1073#1072#1076#1086#1089
      #1069#1083#1100'-'#1057#1072#1083#1100#1074#1072#1076#1086#1088
      #1057#1077#1085#1090'-'#1042#1080#1085#1089#1077#1085#1090
      #1040#1088#1075#1077#1085#1090#1080#1085#1072
      #1041#1086#1083#1080#1074#1080#1103
      #1041#1088#1072#1079#1080#1083#1080#1103
      #1063#1080#1083#1080
      #1050#1086#1083#1091#1084#1073#1080#1103
      #1069#1082#1074#1072#1076#1086#1088
      #1055#1072#1088#1072#1075#1074#1072#1081
      #1057#1091#1088#1080#1085#1072#1084
      #1059#1088#1091#1075#1074#1072#1081
      #1042#1077#1085#1077#1089#1091#1101#1083#1072
      #1043#1072#1081#1072#1085#1072
      #1055#1077#1088#1091
      #1040#1083#1078#1080#1088
      #1070#1040#1056
      #1041#1086#1090#1089#1074#1072#1085#1072
      #1041#1091#1088#1082#1080#1085#1072'-'#1060#1072#1089#1086
      #1041#1091#1088#1091#1085#1076#1080
      #1051#1077#1089#1086#1090#1086
      #1047#1072#1080#1088
      #1047#1072#1084#1073#1080#1103
      #1043#1072#1085#1072
      #1057#1077#1085#1077#1075#1072#1083
      #1050#1086#1090' '#1076'`'#1048#1074#1091#1072#1088
      #1058#1091#1085#1080#1089
      #1052#1072#1083#1080
      #1052#1072#1076#1072#1075#1072#1089#1082#1072#1088
      #1050#1072#1084#1077#1088#1091#1085
      #1063#1072#1076
      #1059#1075#1072#1085#1076#1072
      #1051#1080#1073#1077#1088#1080#1103
      #1052#1086#1079#1072#1084#1073#1080#1082
      #1050#1077#1085#1080#1103
      #1057#1091#1076#1072#1085
      #1057#1074#1072#1079#1080#1083#1077#1085#1076
      #1040#1085#1075#1086#1083#1072
      #1058#1086#1075#1086
      #1047#1080#1084#1073#1072#1073#1074#1077
      #1045#1075#1080#1087#1077#1090
      #1058#1072#1085#1079#1072#1085#1080#1103
      #1053#1080#1075#1077#1088'-'#1053#1080#1075#1077#1088#1080#1103
      #1069#1092#1080#1086#1087#1080#1103
      #1043#1072#1073#1086#1085
      #1057#1100#1077#1088#1072'-'#1051#1077#1086#1085#1077
      #1041#1077#1085#1080#1085
      #1050#1086#1085#1075#1086
      #1043#1074#1080#1085#1077#1103
      'SRL'
      #1052#1072#1088#1086#1082#1082#1086
      #1043#1072#1084#1073#1080#1103
      #1052#1072#1083#1072#1074#1080
      #1071#1087#1086#1085#1080#1103
      #1058#1072#1081#1074#1072#1085#1100
      #1048#1085#1076#1080#1103'-'#1048#1085#1076#1086#1085#1077#1079#1080#1103
      #1041#1072#1085#1075#1083#1072#1076#1077#1096
      #1041#1088#1091#1085#1077#1081
      #1048#1088#1072#1085'-'#1048#1088#1072#1082
      #1048#1086#1088#1076#1072#1085#1080#1103
      #1064#1088#1080'-'#1051#1072#1085#1082#1072
      #1057#1080#1088#1080#1103
      #1070#1078#1085#1072#1103' '#1050#1086#1088#1077#1103
      'IRN'
      #1042#1100#1077#1090#1085#1072#1084
      #1052#1072#1083#1072#1081#1079#1080#1103
      #1057#1072#1091#1076#1086#1074#1089#1082#1072#1103' '#1040#1088#1072#1074#1080#1103
      #1049#1077#1084#1077#1085
      #1050#1091#1074#1077#1081#1090
      #1051#1072#1086#1089
      #1057#1077#1074#1077#1088#1085#1072#1103' '#1050#1086#1088#1077#1103
      #1054#1084#1072#1085
      #1055#1072#1082#1080#1089#1090#1072#1085
      #1060#1080#1083#1080#1087#1087#1080#1085#1099
      #1050#1080#1090#1072#1081
      #1057#1080#1085#1075#1072#1087#1091#1088
      #1052#1072#1091#1088#1080#1090#1091#1089
      #1052#1100#1103#1085#1084#1072
      #1055#1072#1087#1091#1072'-'#1053#1086#1074#1072#1103' '#1043#1074#1080#1085#1077#1103
      #1058#1072#1076#1078#1080#1082#1080#1089#1090#1072#1085
      #1059#1079#1073#1077#1082#1080#1089#1090#1072#1085
      #1050#1072#1090#1072#1088
      #1054#1040#1069
      #1040#1074#1089#1090#1088#1072#1083#1080#1103
      #1053#1086#1074#1072#1103' '#1047#1077#1083#1072#1085#1076#1080#1103
      #1060#1080#1076#1078#1080
      #1057#1086#1083#1086#1084#1086#1085#1086#1074#1099' '#1086#1089#1090#1088#1086#1074#1072
      #1044#1088#1091#1075#1072#1103)
  end
  object ComboBox10: TComboBox
    Left = 72
    Top = 216
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 4
    Text = 'ComboBox10'
    Visible = False
    Items.Strings = (
      'GK'
      'RB'
      'LB'
      'CD'
      'RW'
      'LW'
      'CM'
      'AT')
  end
  object ComboBox3: TComboBox
    Left = 72
    Top = 120
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 5
    Text = 'ComboBox3'
    Visible = False
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '-1'
      '9'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '46'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '40'
      '33'
      '45'
      '34'
      '35'
      '36'
      '37'
      '80'
      '68'
      '148'
      '69'
      '70'
      '51'
      '71'
      '72'
      '73'
      '66'
      '-1'
      '-1'
      '-1'
      '118'
      '-1'
      '-1'
      '-1'
      '-1'
      '56'
      '-1'
      '149'
      '-1'
      '74'
      '79'
      '75'
      '119'
      '-1'
      '81'
      '-1'
      '76'
      '152'
      '58'
      '-1'
      '120'
      '38'
      '77'
      '39'
      '88'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1'
      '-1')
  end
  object ComboBox4: TComboBox
    Left = 72
    Top = 168
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 6
    Text = 'ComboBox4'
    Visible = False
  end
  object ComboBox5: TComboBox
    Left = 224
    Top = 96
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 7
    Text = 'ComboBox5'
    Visible = False
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '33'
      '34'
      '35'
      '36'
      '37'
      '38'
      '39'
      '40'
      '41'
      '42'
      '43'
      '44'
      '45'
      '46'
      '47'
      '48'
      '49'
      '50'
      '51'
      '55'
      '60'
      '62'
      '64'
      '65'
      '66'
      '67'
      '69'
      '71'
      '73'
      '75'
      '76'
      '77'
      '78'
      '79')
  end
  object ComboBox6: TComboBox
    Left = 224
    Top = 120
    Width = 145
    Height = 21
    ItemHeight = 13
    TabOrder = 8
    Text = 'ComboBox6'
    Visible = False
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '9'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '46'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '40'
      '33'
      '45'
      '34'
      '35'
      '36'
      '37'
      '80'
      '68'
      '148'
      '69'
      '70'
      '51'
      '71'
      '72'
      '73'
      '52'
      '118'
      '56'
      '149'
      '74'
      '79'
      '75'
      '119'
      '81'
      '76'
      '58'
      '120'
      '38'
      '77'
      '39'
      '88')
  end
  object ComboBox11: TComboBox
    Left = 512
    Top = 83
    Width = 225
    Height = 28
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ItemHeight = 20
    ParentFont = False
    TabOrder = 9
    Text = 'ComboBox11'
    OnChange = ComboBox11Change
    Items.Strings = (
      '0'
      '1'
      '2'
      '3'
      '4'
      '5'
      '6'
      '7'
      '8'
      '10'
      '11'
      '12'
      '13'
      '14'
      '15'
      '16'
      '17'
      '18'
      '19'
      '20'
      '21'
      '22'
      '23'
      '24'
      '25'
      '26'
      '27'
      '28'
      '29'
      '30'
      '31'
      '32'
      '33'
      '34'
      '35'
      '36'
      '37'
      '38'
      '39'
      '40'
      '41'
      '42'
      '43'
      '44'
      '45'
      '46'
      '47'
      '48'
      '49'
      '50'
      '51'
      '55'
      '60'
      '62'
      '64'
      '65'
      '66'
      '67'
      '69'
      '71'
      '73'
      '75'
      '76'
      '77'
      '78'
      '79')
  end
  object ComboBox12: TComboBox
    Left = 512
    Top = 109
    Width = 225
    Height = 28
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clWindowText
    Font.Height = -16
    Font.Name = 'MS Sans Serif'
    Font.Style = []
    ItemHeight = 20
    ParentFont = False
    TabOrder = 10
    Text = 'ComboBox12'
  end
  object Button3: TButton
    Left = 440
    Top = 216
    Width = 75
    Height = 25
    Caption = 'Button3'
    TabOrder = 11
    Visible = False
    OnClick = Button3Click
  end
  object ProgressBar1: TProgressBar
    Left = 0
    Top = 304
    Width = 748
    Height = 33
    Align = alBottom
    TabOrder = 12
    Visible = False
  end
  object lstSwosPath: TDirectoryListBox
    Left = 8
    Top = 32
    Width = 377
    Height = 257
    ItemHeight = 16
    TabOrder = 13
    OnChange = lstSwosPathChange
  end
  object btnCareer: TButton
    Left = 392
    Top = 32
    Width = 241
    Height = 41
    Caption = 'GENERATE NEW WORLD'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clBlack
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 14
    OnClick = btnCareerClick
  end
  object btnRecordTeamToCareer: TButton
    Left = 392
    Top = 144
    Width = 241
    Height = 41
    Caption = 'RECORD TEAM TO CAREER'
    Font.Charset = DEFAULT_CHARSET
    Font.Color = clBlack
    Font.Height = -13
    Font.Name = 'MS Sans Serif'
    Font.Style = [fsBold]
    ParentFont = False
    TabOrder = 15
    OnClick = btnRecordTeamToCareerClick
  end
  object OpenDialog1: TOpenDialog
    Left = 384
    Top = 8
  end
end