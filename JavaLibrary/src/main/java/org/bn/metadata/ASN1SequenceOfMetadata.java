/*
 Copyright 2006-2011 Abdulla Abdurakhmanov (abdulla@latestbit.com)
 
 Licensed under the Apache License, Version 2.0 (the "License");
 you may not use this file except in compliance with the License.
 You may obtain a copy of the License at

 http://www.apache.org/licenses/LICENSE-2.0

 Unless required by applicable law or agreed to in writing, software
 distributed under the License is distributed on an "AS IS" BASIS,
 WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 See the License for the specific language governing permissions and
 limitations under the License.
 */
package org.bn.metadata;

import java.io.InputStream;
import java.io.OutputStream;
import java.lang.reflect.AnnotatedElement;
import java.lang.reflect.ParameterizedType;
import java.lang.reflect.Type;
import org.bn.annotations.ASN1SequenceOf;
import org.bn.coders.ASN1PreparedElementData;
import org.bn.coders.CoderUtils;
import org.bn.coders.DecodedObject;
import org.bn.coders.ElementInfo;
import org.bn.coders.IASN1TypesDecoder;
import org.bn.coders.IASN1TypesEncoder;

public class ASN1SequenceOfMetadata extends ASN1FieldMetadata {

    private boolean isSetOf = false;
    private Type itemClass;
    private ASN1PreparedElementData itemClassMetadata;

    public ASN1SequenceOfMetadata(String name, boolean isSetOf, Type itemClass, AnnotatedElement seqFieldAnnotatedElem) {
        super(name);
        this.isSetOf = isSetOf;
        this.itemClass = itemClass;
        //ParameterizedType tp = (ParameterizedType)itemClass;
        //Class paramType = (Class)tp.getActualTypeArguments()[0];
        Class paramType = CoderUtils.getCollectionType((ParameterizedType) itemClass);

        this.itemClassMetadata = new ASN1PreparedElementData(paramType);
        if (this.itemClassMetadata.getTypeMetadata() != null) {
            this.itemClassMetadata.getTypeMetadata().setParentAnnotated(seqFieldAnnotatedElem);
        }
    }

    public ASN1SequenceOfMetadata(ASN1SequenceOf annotation, Type itemClass, AnnotatedElement seqFieldAnnotatedElem) {
        this(annotation.name(), annotation.isSetOf(), itemClass, seqFieldAnnotatedElem);
    }

    public boolean isSetOf() {
        return this.isSetOf;
    }

    public ASN1PreparedElementData getItemClassMetadata() {
        return itemClassMetadata;
    }

    @Override
    public int encode(IASN1TypesEncoder encoder, Object object, OutputStream stream, ElementInfo elementInfo) throws Exception {
        return encoder.encodeSequenceOf(object, stream, elementInfo);
    }

    @Override
    public DecodedObject<?> decode(IASN1TypesDecoder decoder, DecodedObject<Integer> decodedTag, Class<?> objectClass, ElementInfo elementInfo, InputStream stream) throws Exception {
        return decoder.decodeSequenceOf(decodedTag, objectClass, elementInfo, stream);
    }
}
