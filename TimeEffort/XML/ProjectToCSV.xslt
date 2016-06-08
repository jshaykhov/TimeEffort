<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
    xmlns:msxsl="urn:schemas-microsoft-com:xslt" exclude-result-prefixes="msxsl"
>
    <xsl:output method="text" encoding="iso-8859-1"/>

  <xsl:strip-space elements="*" />
  <xsl:template match="/">
    <xsl:for-each select="/Projects/Project">
      <xsl:value-of select="Id" />, "<xsl:value-of select="Name" />","<xsl:value-of select="Code" />", "<xsl:value-of select="ContractUSD" />", "<xsl:value-of select="ContractUZS" />" ,"<xsl:value-of select="ManagerID" />, "<xsl:value-of select="StartDate" />",
      "<xsl:value-of select="EndDate" />", "<xsl:value-of select="Status" />"
      <xsl:text>&#xD;</xsl:text>
    </xsl:for-each>
                  
    </xsl:template>
    
</xsl:stylesheet>
